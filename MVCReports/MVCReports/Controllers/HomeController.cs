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

            var role = System.Web.HttpContext.Current.Session["role"];

            ServerReport report = new ServerReport();
                  
            //astea tb sa le facem cumva global dada
            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportServerCredentials = new CustomCredentials();        

            reportViewer.ServerReport.ReportPath = "/Test/PieChart";
            reportViewer.ServerReport.ReportServerUrl = new Uri(AppConstants.ServerURL);

            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

                  
            ViewBag.reportView = reportViewer;
            AccuracyViewModel accuracy = new AccuracyViewModel
            {
                ClientNames = db.Accuracy_Setup.ToList().Select(a=>a.CUSTOMER).ToList()
            };

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
    }
}