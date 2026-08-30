using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Models.ViewModels;

namespace NektarPodgorine.Web.Controllers
{
    public class PcelinjaciController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public ActionResult Index()
        {
            var pcelinjaci = Db.Pcelinjaci.OrderBy(p => p.Naziv).ToList();
            return View(pcelinjaci);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pcelinjak = Db.Pcelinjaci.Find(id.Value);
            if (pcelinjak == null)
            {
                return HttpNotFound();
            }

            return View(pcelinjak);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new PcelinjakCreateVM());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PcelinjakCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Db.Pcelinjaci.Add(new Pcelinjak
            {
                Naziv = model.Naziv,
                Mesto = model.Mesto,
                Opis = model.Opis,
                GeografskaSirina = model.GeografskaSirina,
                GeografskaDuzina = model.GeografskaDuzina
            });
            Db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pcelinjak = Db.Pcelinjaci.Find(id.Value);
            if (pcelinjak == null)
            {
                return HttpNotFound();
            }

            return View(new PcelinjakEditVM
            {
                Id = pcelinjak.Id,
                Naziv = pcelinjak.Naziv,
                Mesto = pcelinjak.Mesto,
                Opis = pcelinjak.Opis,
                GeografskaSirina = pcelinjak.GeografskaSirina,
                GeografskaDuzina = pcelinjak.GeografskaDuzina
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PcelinjakEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var pcelinjak = Db.Pcelinjaci.Find(model.Id);
            if (pcelinjak == null)
            {
                return HttpNotFound();
            }

            pcelinjak.Naziv = model.Naziv;
            pcelinjak.Mesto = model.Mesto;
            pcelinjak.Opis = model.Opis;
            pcelinjak.GeografskaSirina = model.GeografskaSirina;
            pcelinjak.GeografskaDuzina = model.GeografskaDuzina;
            Db.SaveChanges();

            return RedirectToAction("Details", new { id = pcelinjak.Id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pcelinjak = Db.Pcelinjaci.Find(id.Value);
            if (pcelinjak == null)
            {
                return HttpNotFound();
            }

            return View(pcelinjak);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pcelinjak = Db.Pcelinjaci.Find(id);
            if (pcelinjak != null)
            {
                Db.Pcelinjaci.Remove(pcelinjak);
                Db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
