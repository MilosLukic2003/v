using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Models;

namespace NektarPodgorine.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var najnovije = db.Vesti
                .OrderByDescending(v => v.DatumObjave)
                .Take(3)
                .ToList();

            return View(najnovije);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}