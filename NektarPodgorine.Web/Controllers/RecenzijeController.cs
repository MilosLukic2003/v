using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Models.ViewModels;

namespace NektarPodgorine.Web.Controllers
{
    [Authorize]
    public class RecenzijeController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecenzijaCreateVM model)
        {
            if (Db.Proizvodi.Find(model.ProizvodId) == null)
            {
                return HttpNotFound();
            }

            var korisnikId = User.Identity.GetUserId();

            if (Db.Recenzije.Any(r => r.ProizvodId == model.ProizvodId && r.KorisnikId == korisnikId))
            {
                TempData["Poruka"] = "Već ste ostavili recenziju za ovaj proizvod. Možete je izmeniti.";
                return NazadNaProizvod(model.ProizvodId);
            }

            if (!ModelState.IsValid)
            {
                TempData["Poruka"] = "Recenzija nije sačuvana - proverite ocenu i tekst.";
                return NazadNaProizvod(model.ProizvodId);
            }

            Db.Recenzije.Add(new Recenzija
            {
                ProizvodId = model.ProizvodId,
                KorisnikId = korisnikId,
                Ocena = model.Ocena,
                Sadrzaj = model.Sadrzaj,
                DatumKreiranja = DateTime.Now
            });
            Db.SaveChanges();

            return NazadNaProizvod(model.ProizvodId);
        }

        public ActionResult Edit(int id)
        {
            var recenzija = Db.Recenzije.Find(id);
            if (recenzija == null)
            {
                return HttpNotFound();
            }

            if (recenzija.KorisnikId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(new RecenzijaEditVM
            {
                Id = recenzija.Id,
                ProizvodId = recenzija.ProizvodId,
                ProizvodNaziv = recenzija.Proizvod.Naziv,
                Ocena = recenzija.Ocena,
                Sadrzaj = recenzija.Sadrzaj
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecenzijaEditVM model)
        {
            var recenzija = Db.Recenzije.Find(model.Id);
            if (recenzija == null)
            {
                return HttpNotFound();
            }

            if (recenzija.KorisnikId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (!ModelState.IsValid)
            {
                model.ProizvodNaziv = recenzija.Proizvod.Naziv;
                return View(model);
            }

            recenzija.Ocena = model.Ocena;
            recenzija.Sadrzaj = model.Sadrzaj;
            Db.SaveChanges();

            return NazadNaProizvod(recenzija.ProizvodId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var recenzija = Db.Recenzije.Find(id);
            if (recenzija == null)
            {
                return HttpNotFound();
            }

            if (recenzija.KorisnikId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var proizvodId = recenzija.ProizvodId;
            Db.Recenzije.Remove(recenzija);
            Db.SaveChanges();

            return NazadNaProizvod(proizvodId);
        }

        private ActionResult NazadNaProizvod(int proizvodId)
        {
            return RedirectToAction("Details", "Proizvodi", new { id = proizvodId });
        }
    }
}
