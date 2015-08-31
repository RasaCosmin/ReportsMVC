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

namespace MVCReports.Controllers
{

    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {

            ServerReport report = new ServerReport();
                  
            //astea tb sa le facem cumva global
            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportServerCredentials = new CustomCredentials();        

            reportViewer.ServerReport.ReportPath = "/Test/PieChart";
            reportViewer.ServerReport.ReportServerUrl = new Uri(AppConstants.ServerURL);

            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

                  
            ViewBag.reportView = reportViewer;
            return View();
        }
    }
}