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

        public ActionResult Index()
        {

            ServerReport report = new ServerReport();
                  

            var reportViewer = new ReportViewer();

            reportViewer.ServerReport.ReportServerCredentials = new CustomCredentials("andrei", "P@ssw0rd", "http://172.17.2.2");

            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportPath = "/TableWithParameters";
            reportViewer.ServerReport.ReportServerUrl = new Uri("http://172.17.2.2/reportserver");

            
                                 
            ViewBag.reportView = reportViewer;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}