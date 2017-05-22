using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataBaseMigrator.Models;
using DataBaseMigrator.Infrastructure;
using DataBaseMigrator.Interface;
using System.Data;

namespace DataBaseMigrator.Controllers
{
    public class MigratorController : CoreController
    {
        private IRepositoryCore maindirectory;
        public MigratorController(IRepositoryCore maindirectory)
        {
            this.maindirectory = maindirectory;
        }
        // GET: Authentication
        [HttpGet]
        public ActionResult Enter()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult ProgressShow()
        {
            if(!ConnectionStringManger.ExistConnect())
            {
                DeleteCookies();
                return RedirectToAction("Enter");
            }
            return View(maindirectory.GetAllTables());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enter(EnterStatus f)
        {
            var status = true;
             status &= RepositoryCore.TestConnection(f.GetCampusConnectionString());
             status &= RepositoryCore.TestConnection(f.GetVkdConnectionString());
             if(!status)
            {
                return Json("Connection to Database Fail", JsonRequestBehavior.AllowGet);
            }
            ConnectionStringManger.CampusBd = f.GetCampusConnectionString();
            ConnectionStringManger.VkdBd = f.GetVkdConnectionString();
            FormsAuthentication.SetAuthCookie("Availeble", true);
            return Json(new { url = Url.Action("ProgressShow", "Migrator") });
        }

        [HttpGet]
        [Authorize]
        public ActionResult Out()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Enter");
        }
    }
}