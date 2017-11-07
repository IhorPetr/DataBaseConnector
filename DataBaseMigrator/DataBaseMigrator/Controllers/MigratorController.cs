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
using System.IO;
using System.Threading.Tasks;

namespace DataBaseMigrator.Controllers
{
    public class MigratorController : CoreController
    {
        private IMigratorRepository maindirectory;
        public MigratorController(IMigratorRepository maindirectory)
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
             status &= MigratorRepository.TestConnection(f.GetCampusConnectionString());
             status &= MigratorRepository.TestConnection(f.GetVkdConnectionString());
             if(!status)
            {
                return Json("Ошибка соединения с базой", JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ProgressShow(string[] g)
        {
            try
            {
               maindirectory.UpdateCampusDatabase(g);
            }
            catch(Exception ex)
            {
                NLogCore.LogAplicationError(ex.Message);
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json("Імпорт даних завершен успішно", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize]
        public ActionResult GetAplicationStatus()
        {
           
            string file_path = Server.MapPath("~/Logs/InfoLog.log");
            if (!System.IO.File.Exists(file_path))
            {
              return Content("<script language='javascript' type='text/javascript'>" +
                        "alert('File doesn`t exist!');" +
                        "window.location.href='/Migrator/ProgressShow'" +
                        "</script>");
            }
            byte[] mas = System.IO.File.ReadAllBytes(file_path);
            string file_type = "application/octet-stream";
            string file_name = "InfoLog.log";
            return File(mas, file_type, file_name);
        }
        [HttpGet]
        [Authorize]
        public ActionResult GetAplicationError()
        {
            string file_path = Server.MapPath("~/Logs/ErrorLog.log");
            if (!System.IO.File.Exists(file_path))
            {
                return Content("<script language='javascript' type='text/javascript'>" +
                        "alert('File doesn`t exist!');" +
                        "window.location.href='/Migrator/ProgressShow'"+
                        "</script>");
            }
            byte[] mas = System.IO.File.ReadAllBytes(file_path);

            string file_type = "application/octet-stream";
            string file_name = "ErrorLog.log";
            return File(mas, file_type, file_name);
        }
        [HttpGet]
        [Authorize]
        public ActionResult ProgressBarCount(string y)
        {
            var t= maindirectory.GetProgressBarCount(y);
            return Json(t, JsonRequestBehavior.AllowGet);
        }
    }
}