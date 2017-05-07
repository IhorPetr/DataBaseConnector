using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataBaseMigrator.Models;

namespace DataBaseMigrator.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        [HttpGet]
        public ActionResult Enter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Enter(EnterStatus f)
        {
            if(ModelState.IsValid)
            {
                return View(f);
            }
            return View(f);
        }
    }
}