using Microsoft.AspNet.Identity.EntityFramework;
using MVCReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCReports.Controllers
{
    public class RoleController : Controller
    {

        private ApplicationDbContext context;
        
        public RoleController()
        {
            context = new ApplicationDbContext();
        }


        // GET: Role
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            var users = context.Users.ToList();
            
            return View(roles);
        }

        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}