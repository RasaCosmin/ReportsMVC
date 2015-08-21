using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;

namespace MVCReports.Helpers
{
    public class CustomCredentials : IReportServerCredentials
    {
        private string _username;
        private string _password;
        private string _domainName;

        public CustomCredentials(string username, string password, string domain)
        {
            _username = username;
            _password = password;
            _domainName = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(_username, _password, _domainName);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = password = authority = null;
            return false;
        }
    }
}