using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Principal;
using MVCReports.Helpers;
using MVCReports.Context;
using MVCReports.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MVCReports.Controllers
{

    public class HomeController : Controller
    {
        private EntityContext db;
        private static string _reportType;

        public HomeController()
        {
            db = new EntityContext();
            _reportType = "Email";
        }

        [Authorize]
        public async Task<ActionResult> Index(AccuracyViewModel response = null, string reportType="")
        {
            if (reportType != "")
                _reportType = reportType;

            var inSession = await VerifyAndSetUserRole();

            if(!inSession)
            {
                VerifySession();
            }


            var accuracy = GenerateAccuracy(response); 

            ViewBag.Type = _reportType;

            return View(accuracy);
        }

        private AccuracyViewModel GenerateAccuracy(AccuracyViewModel response)
        {
            AccuracyViewModel accuracy = response;
            var isAll = false;

            if (response.Customers.Count == 0 && (response.StartDate == null || response.EndDate == null))
            {
                var dbService = new DatabaseService();
                accuracy = new AccuracyViewModel();

                var today = DateTime.Today.AddMonths(-6);

                switch (_reportType)
                {
                    case "Email":

                        accuracy.EndDate = today.ToString("dd-MM-yyyy");
                        accuracy.StartDate = today.AddMonths(-1).ToString("dd-MM-yyyy");

                        break;
                    case "Pie":

                        accuracy.EndDate = today.ToString("dd-MM-yyyy");
                        accuracy.StartDate = today.AddMonths(-1).ToString("dd-MM-yyyy");

                        break;
                    case "Accuracy":

                        accuracy.EndDate = today.ToString("dd-MM-yyyy");
                        accuracy.StartDate = today.AddMonths(-1).ToString("dd-MM-yyyy");

                        break;
                    case "Stacked":

                        accuracy.EndDate = today.ToString("dd-MM-yyyy");
                        accuracy.StartDate = today.AddMonths(-1).ToString("dd-MM-yyyy");

                        break;
                }


                var customersName = dbService.GetList(_reportType);

                foreach (var name in customersName)
                {
                    var customer = new Customer
                    {
                        Name = name,
                        Checked = false
                    };

                    accuracy.Customers.Add(customer);
                }

                isAll = true;
            }

            var reportViewer = ConstructReportView(accuracy, isAll);

            ViewBag.reportView = reportViewer;

            return accuracy;
        }

        private void VerifySession()
        {
            var role = System.Web.HttpContext.Current.Session["role"];
            if(role != null)
            {
                System.Web.HttpContext.Current.Session.Remove("role");
            }
        }

        private async Task<bool> VerifyAndSetUserRole()
        {
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = UserManager.Users.Where(u => u.UserName.Equals(User.Identity.Name)).First();

            if (user == null)
                return false;

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = await RoleManager.FindByIdAsync(user.Roles.First().RoleId);

            if (role == null)
                return false;

            var d = role.Name;

            System.Web.HttpContext.Current.Session["role"] = d;


            return true;
        }

        private ReportViewer ConstructReportView(AccuracyViewModel model, bool selectAll)
        {
            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportServerCredentials = new CustomCredentials();
           
            reportViewer.ServerReport.ReportPath = "/Genpact/Reports/VerticalStackedBar3Months";
            reportViewer.ServerReport.ReportServerUrl = new Uri(AppConstants.ServerURL);
            
            var list = new List<ReportParameter>();

            var p1 = new ReportParameter("StartDate", model.StartDate);
            list.Add(p1);
           
            var p2 = new ReportParameter("Last3MonthDate", model.EndDate);
            list.Add(p2);

            var projects = new List<string>();

            foreach (var customer in model.Customers)
                if (selectAll || customer.Checked)
                    projects.Add(customer.Name);

            var projectsNames = projects.ToArray();

            var p3 = new ReportParameter("Project", projectsNames);          
            list.Add(p3);


            reportViewer.ServerReport.SetParameters(list);
            reportViewer.ServerReport.Refresh();
            reportViewer.ShowParameterPrompts = false;
            reportViewer.Width = Unit.Pixel(720);
            reportViewer.Height = Unit.Pixel(540);

            return reportViewer;
        }

        public ActionResult GenerateReport(AccuracyViewModel response)
        {
           return RedirectToAction("Index", response);
        }

        [HttpPost]
        public JsonResult GenerateReport(string customers)
        {
            var convertedCusomers = JsonConvert.DeserializeObject<AccuracyViewModel>(customers);

            if (customers != null)
            {
                return Json(customers);
            }
            else
            {
                return Json("An Error Has occoured");
            }
        }
    }
}