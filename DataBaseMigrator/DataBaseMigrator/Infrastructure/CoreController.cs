using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBaseMigrator.Infrastructure
{
    public class CoreController : Controller
    {
        protected void DeleteCookies()
        {
            var t = new HttpCookie("Cookies");
            t.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(t);
        }
    }
}