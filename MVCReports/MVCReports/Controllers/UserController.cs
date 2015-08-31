using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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

        public UserRoleController()
        {
            context = new ApplicationDbContext();
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

    }
}
