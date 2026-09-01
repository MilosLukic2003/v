using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Infrastructure;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Models.ViewModels;

namespace NektarPodgorine.Web.Areas.Admin.Controllers
{
    [Autorizacija(Roles = "Admin")]
    public class AdminProizvodiController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public ActionResult Index()
        {
            var proizvodi = Db.Proizvodi
                .Include(p => p.Kategorija)
                .OrderBy(p => p.Naziv)
                .ToList();

            return View(proizvodi);
        }

        public ActionResult Kategorije()
        {
            var kategorije = Db.Kategorije
                .Include(k => k.Proizvodi)
                .OrderBy(k => k.Naziv)
                .ToList();

            return View(kategorije);
        }

        public ActionResult KreirajKategoriju()
        {
            return View(new KategorijaVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KreirajKategoriju(KategorijaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Db.Kategorije.Add(new KategorijaProizvoda
            {
                Naziv = model.Naziv,
                Opis = model.Opis
            });
            Db.SaveChanges();

            return RedirectToAction("Kategorije");
        }

        public ActionResult IzmeniKategoriju(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var kategorija = Db.Kategorije.Find(id.Value);
            if (kategorija == null)
            {
                return HttpNotFound();
            }

            return View(new KategorijaVM
            {
                Id = kategorija.Id,
                Naziv = kategorija.Naziv,
                Opis = kategorija.Opis
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzmeniKategoriju(KategorijaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var kategorija = Db.Kategorije.Find(model.Id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }

            kategorija.Naziv = model.Naziv;
            kategorija.Opis = model.Opis;
            Db.SaveChanges();

            return RedirectToAction("Kategorije");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ObrisiKategoriju(int id)
        {
            var kategorija = Db.Kategorije.Include(k => k.Proizvodi).SingleOrDefault(k => k.Id == id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }

            if (kategorija.Proizvodi != null && kategorija.Proizvodi.Any())
            {
                TempData["Greska"] = "Kategorija sadrži proizvode i ne može biti obrisana.";
                return RedirectToAction("Kategorije");
            }

            Db.Kategorije.Remove(kategorija);
            Db.SaveChanges();

            return RedirectToAction("Kategorije");
        }
    }
}
