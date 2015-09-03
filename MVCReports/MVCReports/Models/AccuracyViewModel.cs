using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCReports.Models
{
    public class AccuracyViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Email { get; set; }

        public List<Customer> Customers { get; set; }

        public AccuracyViewModel(){
            Customers = new List<Customer>();   
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        [Display(Name = "Some Text")]
        public bool Checked { get; set; }
    }
}