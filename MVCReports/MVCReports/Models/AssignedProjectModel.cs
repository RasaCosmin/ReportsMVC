using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCReports.Models
{
    public class AssignedProjectModel
    {
        public Dictionary<string, List<Customer>> AssignedProject { get; set; }
        public string UserName { get; set; }
    }
}
