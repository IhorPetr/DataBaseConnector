using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataBaseMigrator.Models;
using DataBaseMigrator.Infrastructure;
using System.Data;

namespace DataBaseMigrator.Controllers
{
    public class AuthenticationController : Controller
    {
        private MigratorCore maindirectory;
        // GET: Authentication
        [HttpGet]
        public ActionResult Enter()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult ProgressShow()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enter(EnterStatus f)
        {
            var h = new EmployeeTableContext<c_eE>(f.GetVkdConnectionString());
            try
            {
                var u = h.EmployeeTarget.ToList();        
            }
            catch(Exception ex)
            {

            }
            var status = true;
             status &= f.TestConnection(f.GetCampusConnectionString());
             status &= f.TestConnection(f.GetVkdConnectionString());
             if(!status)
            {
                return Json("Connection to Database Fail", JsonRequestBehavior.AllowGet);
            }
            maindirectory = new MigratorCore(f.GetCampusConnectionString(),f.GetVkdConnectionString());
            FormsAuthentication.SetAuthCookie("Availeble", true);
            return Json(new { url = Url.Action("ProgressShow", "Authentication") });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Out()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Enter");
        }
    }
}