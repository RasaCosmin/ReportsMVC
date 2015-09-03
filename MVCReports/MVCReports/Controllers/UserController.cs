using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MVCReports.Context;
using MVCReports.Helpers;
using MVCReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCReports.Controllers
{
    public class UserRoleController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext context;
        private EntityContext db;

        public UserRoleController()
        {
            context = new ApplicationDbContext();
            db = new EntityContext();
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            var userList = new List<UserWithRole>();
            var users = context.Users.ToList();

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            foreach (var u in users)
            {
                var user = new UserWithRole();
                user.UserName = u.UserName;
                user.Email = u.Email;
                user.PhoneNumber = u.PhoneNumber;
                user.Id = u.Id;

                if (u.Roles.Count == 0)
                    user.Role = "";
                else
                {
                    var role = await RoleManager.FindByIdAsync(u.Roles.First().RoleId);
                    user.Role = role.Name;
                }
                userList.Add(user);
            }         
            return View(userList);
        }

        public ActionResult ModifyRole(string id)
        {
            var u = context.Users.Where(us => us.Id == id).FirstOrDefault();
            var user = new UserWithRole();
            user.UserName = u.UserName;
            user.Email = u.Email;
            user.PhoneNumber = u.PhoneNumber;
            user.Id = u.Id;

            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View(user);
        }
        
        [HttpPost]
        public async Task<ActionResult> ModifyRole(string id, string roles)
        {
            var user = UserManager.Users.Where(u => u.Id.Equals(id)).ToList()[0];

            if (user.Roles.Count > 0) {
                var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role = await RoleManager.FindByIdAsync(user.Roles.First().RoleId);
                UserManager.RemoveFromRole(id, role.Name);
            }

            await UserManager.AddToRoleAsync(id, roles);
            return RedirectToAction("index");
        }

        public ActionResult AssingProjectToUser(string userName)
        {
            var projectList = new Dictionary<string, List<Customer>>();                              
           
            projectList.Add("Pie",ConstructCustomerList("Pie", userName));
            projectList.Add("Stacked", ConstructCustomerList("Stacked", userName));
            projectList.Add("Accuracy", ConstructCustomerList("Accuracy", userName));

            var assignedProjects= new AssignedProjectModel() { AssignedProject = projectList };

            return View(assignedProjects);
        }

        private List<Customer> ConstructCustomerList(string table, string userName)
        {
            var list = new List<Customer>();

            var dbService = new DatabaseService();
            var customers = dbService.GetList(table);

            var userProjects = db.UserProject.Where(e => e.UserName == userName && e.Type == table).Select(e => e.CustomerName).ToList();

            foreach(var c in customers)
            {
                var customer = new Customer { Checked = false, Name = c };
                
                if(userProjects.Contains(c))
                {
                    customer.Checked = true;
                }

                list.Add(customer);
            }


            return list;
        }

        [HttpPost]
        public ActionResult AssingProjectToUser(AssignedProjectModel projects)
        {
            var t  = ModelState.IsValid;
            return RedirectToAction("index");
        }
    }
}
