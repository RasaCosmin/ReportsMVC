using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCReports.Models
{
    public class UserWithRole:ApplicationUser
    {
        public string Role { get; set; }
    }
}
