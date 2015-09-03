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
        public static string ReportType;

        public HomeController()
        {
            db = new EntityContext();
        }

        [Authorize]
        public async Task<ActionResult> Index(AccuracyViewModel response = null, string reportType="")
        {
            var inSession = await VerifyAndSetUserRole();

            if(!inSession)
            {
                VerifySession();
            }

            SetReportType(reportType);

            var accuracy = GenerateAccuracy(response); 

            ViewBag.Type = ReportType;

            return View(accuracy);
        }

        private void SetReportType(string reportType)
        {
            if (ReportType == null || ReportType == "")
            {
                var role = System.Web.HttpContext.Current.Session["role"];
                if (role.ToString() == "Admin")
                    ReportType = "Email";
                else reportType = "Pie";
            }

            if (reportType != "")
                ReportType = reportType;
        }

        private AccuracyViewModel GenerateAccuracy(AccuracyViewModel response)
        {
            AccuracyViewModel accuracy = response;
            var isAll = false;

            if (response.Customers.Count == 0 && (response.StartDate == null || response.EndDate == null))
            {
              
                accuracy = new AccuracyViewModel();

                var today = DateTime.Today.AddMonths(-6);
                var customersName = GetCustomers();

                switch (ReportType)
                {
                    case "Email":

                        accuracy.EndDate = today.ToString("dd-MM-yyyy");
                        accuracy.StartDate = today.AddMonths(-1).ToString("dd-MM-yyyy");

                        if (customersName.FirstOrDefault().Equals("<>"))
                        {
                            customersName.Remove("<>");
                        }

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

        private List<string> GetCustomers()
        {
            var role = System.Web.HttpContext.Current.Session["role"];
            var dbService = new DatabaseService();

            if (role.ToString() == "Admin")
                return dbService.GetList(ReportType);
            else
            {
                var customers = db.UserProject.Where(e => e.UserName == User.Identity.Name && e.Type == ReportType).Select(r => r.CustomerName).ToList();
                return customers; 
            }
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
            reportViewer.ServerReport.ReportServerUrl = new Uri(AppConstants.ServerURL);

            var list = new List<ReportParameter>();

            switch (ReportType)
            {
                case "Email":
                    reportViewer.ServerReport.ReportPath = "/Genpact/Reports/EmailOffice365";
                    var emailStartDate = new ReportParameter("pStartDate", model.StartDate);
                    list.Add(emailStartDate);

                    var emailEndDate = new ReportParameter("pEndDate", model.EndDate);
                    list.Add(emailEndDate);

                    var email = model.Customers.FirstOrDefault(e => e.Checked == true); 
                    
                    var emailsParam = new ReportParameter("pEmailAddress", email != null ? email.Name : "" );
                    list.Add(emailsParam);
                    break;
                case "Pie":
                    reportViewer.ServerReport.ReportPath = "/Genpact/Reports/PieChart";
                    var pieStartDate = new ReportParameter("StartDate", model.StartDate);
                    list.Add(pieStartDate);

                    var pieEndDate = new ReportParameter("EndDate", model.EndDate);
                    list.Add(pieEndDate);

                    var pieCustomers = model.Customers;
                    var pieProjects = new string[pieCustomers.Count];

                    for (var i = 0; i < pieCustomers.Count; i++)
                        if (selectAll || pieCustomers[i].Checked)
                            pieProjects[i] = pieCustomers[i].Name;

                    var pieProjectsParam = new ReportParameter("Project", pieProjects);
                    list.Add(pieProjectsParam);
                    break;
                case "Accuracy":
                    reportViewer.ServerReport.ReportPath = "/Genpact/Reports/VerticalBar_AccuracyStatistik";
                    var acc = model.Customers.FirstOrDefault(e => e.Checked == true);
                    var accParam = new ReportParameter("pCustomer", acc!=null?acc.Name:"");
                    list.Add(accParam);
                    break;
                case "Stacked":
                    reportViewer.ServerReport.ReportPath = "/Genpact/Reports/VerticalStackedBar3Months";
                    var stackedStartDate = new ReportParameter("StartDate", model.StartDate);
                    list.Add(stackedStartDate);

                    var customers = model.Customers;
                    var projects = new string[customers.Count];

                    for (var i = 0; i < customers.Count; i++)
                        if (selectAll || customers[i].Checked)
                            projects[i] = customers[i].Name;

                    var stackedProjects = new ReportParameter("Project", projects);
                    list.Add(stackedProjects);
                    break;
            }         

            reportViewer.ServerReport.SetParameters(list);
            reportViewer.ServerReport.Refresh();
            reportViewer.ShowParameterPrompts = false;
            reportViewer.Width = Unit.Pixel(720);
            reportViewer.Height = Unit.Pixel(540);

            return reportViewer;
        }

        //public ActionResult GenerateReport(AccuracyViewModel response)
        //{
        //   return RedirectToAction("Index", response);
        //}

        [HttpPost]
        public PartialViewResult GenerateReport(string customers)
        {
            if (customers == null || customers.Equals("[]"))
            {
                return PartialView("_ReportLayout");
            }

            var reportModel = JsonConvert.DeserializeObject<AccuracyViewModel>(customers);
            var convertedCustomers = reportModel.Customers.Where(r => r.Checked).ToList();
            reportModel.Customers = convertedCustomers;
            var accuracy = GenerateAccuracy(reportModel);

            return PartialView("_ReportLayout");
        }
    }
}