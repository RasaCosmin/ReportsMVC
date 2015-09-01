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

namespace MVCReports.Controllers
{

    public class HomeController : Controller
    {
        private EntityContext db;

        public HomeController()
        {
            db = new EntityContext();
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {

            var inSession = await VerifyAndSetUserRole();

            if(!inSession)
            {
                VerifySession();
            }

            ServerReport report = new ServerReport();
                  
            //astea tb sa le facem cumva global dada
            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportServerCredentials = new CustomCredentials();        

            reportViewer.ServerReport.ReportPath = "/Test/PieChart";
            reportViewer.ServerReport.ReportServerUrl = new Uri(AppConstants.ServerURL);

            //reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Pixel(100);
            reportViewer.Height = Unit.Pixel(100);
        
            var list = new List<ReportParameter>();

            var p1 = new ReportParameter("StartDate", "13-03-2015");
            var p2 = new ReportParameter("EndDate", "13-06-2015");

            var projects = new string[] { "3M", "AHOLD" };

            var p3 = new ReportParameter("Project",projects);

            list.Add(p1);
            list.Add(p2);
            list.Add(p3);


            reportViewer.ServerReport.SetParameters(list);
            reportViewer.ServerReport.Refresh();
            reportViewer.ShowParameterPrompts = false;

            var s = reportViewer.ServerReport.GetDataSources();

          

            ViewBag.reportView = reportViewer;
            AccuracyViewModel accuracy = new AccuracyViewModel();
            var customersName = db.Accuracy_Setup.ToList().Select(a => a.CUSTOMER).ToList();

            foreach(var name in customersName)
            {
                var customer = new Customer {
                    Name = name,
                    Checked = false
                };

                accuracy.Customers.Add(customer);
            }

            var today = DateTime.Today;
            accuracy.EndDate = today.ToString("MM-dd-yyyy");
            accuracy.StartDate = today.AddMonths(-1).ToString("MM-dd-yyyy");

            return View(accuracy);
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

        public ActionResult GenerateReport(AccuracyViewModel response)
        {
            var t = 0;
            return View();
        }
    }
}