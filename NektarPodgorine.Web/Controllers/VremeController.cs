using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Services;

namespace NektarPodgorine.Web.Controllers
{
    public class VremeController : Controller
    {
        private readonly WeatherApiService vreme = new WeatherApiService();

        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public async Task<ActionResult> Trenutno(int pcelinjakId)
        {
            var pcelinjak = Db.Pcelinjaci.Find(pcelinjakId);
            if (pcelinjak == null)
            {
                return HttpNotFound();
            }

            if (!vreme.Konfigurisan)
            {
                return Json(new { greska = "Weather API nije podešen." }, JsonRequestBehavior.AllowGet);
            }

            var info = await vreme.TrenutnoVreme(pcelinjak.GeografskaSirina, pcelinjak.GeografskaDuzina);
            if (info == null)
            {
                return Json(new { greska = "Prognoza trenutno nije dostupna." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                mesto = pcelinjak.Mesto,
                temperatura = Math.Round(info.Temperatura, 1),
                vlaznost = info.Vlaznost,
                vetar = Math.Round(info.BrzinaVetra, 1),
                opis = info.Opis,
                ikonica = info.Ikonica
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
