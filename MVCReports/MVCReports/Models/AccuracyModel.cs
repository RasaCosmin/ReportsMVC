using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCReports.Models
{
    public partial class AccuracyModel
    {
        public int ID { get; set; }
        public string CUSTOMER { get; set; }
        public string FIL { get; set; }
        public string SP { get; set; }
        public string TABELL { get; set; }
        public string KOLUMNER { get; set; }
        public Nullable<int> AKTIV { get; set; }
        public Nullable<int> NEW_REPORT { get; set; }
    }

    public class AccuraciesModel
    {
        public List<AccuracyModel> Accuracies { get; set; }
    }
}