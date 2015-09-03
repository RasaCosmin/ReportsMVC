using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCReports.Models
{
    public class UserProjectsModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public string CustomerName { get; set; }
    }
}
