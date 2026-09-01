using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Infrastructure;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Models.ViewModels;

namespace NektarPodgorine.Web.Controllers
{
    public class ProizvodiController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public ActionResult Index(int? kategorijaId)
        {
            var proizvodi = Db.Proizvodi.Include(p => p.Kategorija).AsQueryable();

            if (kategorijaId.HasValue)
            {
                proizvodi = proizvodi.Where(p => p.KategorijaId == kategorijaId.Value);
            }

            PopuniKategorije(kategorijaId);
            ViewBag.KategorijaId = kategorijaId;

            return View(proizvodi.OrderBy(p => p.Naziv).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var proizvod = Db.Proizvodi
                .Include(p => p.Kategorija)
                .Include(p => p.Recenzije.Select(r => r.Korisnik))
                .SingleOrDefault(p => p.Id == id.Value);

            if (proizvod == null)
            {
                return HttpNotFound();
            }

            return View(proizvod);
        }

        [Autorizacija(Roles = "Admin")]
        public ActionResult Create()
        {
            PopuniKategorije();
            return View(new ProizvodCreateVM());
        }

        [HttpPost]
        [Autorizacija(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProizvodCreateVM model)
        {
            var putanjaSlike = SacuvajSliku(model.Slika);

            if (!ModelState.IsValid)
            {
                PopuniKategorije(model.KategorijaId);
                return View(model);
            }

            var proizvod = new Proizvod
            {
                Naziv = model.Naziv,
                Opis = model.Opis,
                Cena = model.Cena,
                JedinicaMere = model.JedinicaMere,
                KolicinaNaStanju = model.KolicinaNaStanju,
                ImageUrl = putanjaSlike ?? model.ImageUrl,
                KategorijaId = model.KategorijaId,
                DatumDodavanja = DateTime.Now,
                KreiraoId = User.Identity.GetUserId()
            };

            Db.Proizvodi.Add(proizvod);
            Db.SaveChanges();

            return RedirectToAction("Details", new { id = proizvod.Id });
        }

        [Autorizacija(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var proizvod = Db.Proizvodi.Find(id.Value);
            if (proizvod == null)
            {
                return HttpNotFound();
            }

            PopuniKategorije(proizvod.KategorijaId);

            return View(new ProizvodEditVM
            {
                Id = proizvod.Id,
                Naziv = proizvod.Naziv,
                Opis = proizvod.Opis,
                Cena = proizvod.Cena,
                JedinicaMere = proizvod.JedinicaMere,
                KolicinaNaStanju = proizvod.KolicinaNaStanju,
                ImageUrl = proizvod.ImageUrl,
                KategorijaId = proizvod.KategorijaId
            });
        }

        [HttpPost]
        [Autorizacija(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProizvodEditVM model)
        {
            var putanjaSlike = SacuvajSliku(model.Slika);

            if (!ModelState.IsValid)
            {
                PopuniKategorije(model.KategorijaId);
                return View(model);
            }

            var proizvod = Db.Proizvodi.Find(model.Id);
            if (proizvod == null)
            {
                return HttpNotFound();
            }

            proizvod.Naziv = model.Naziv;
            proizvod.Opis = model.Opis;
            proizvod.Cena = model.Cena;
            proizvod.JedinicaMere = model.JedinicaMere;
            proizvod.KolicinaNaStanju = model.KolicinaNaStanju;
            proizvod.ImageUrl = putanjaSlike ?? model.ImageUrl;
            proizvod.KategorijaId = model.KategorijaId;

            Db.SaveChanges();

            return RedirectToAction("Details", new { id = proizvod.Id });
        }

        [Autorizacija(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var proizvod = Db.Proizvodi
                .Include(p => p.Kategorija)
                .SingleOrDefault(p => p.Id == id.Value);

            if (proizvod == null)
            {
                return HttpNotFound();
            }

            return View(proizvod);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Autorizacija(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var proizvod = Db.Proizvodi.Find(id);
            if (proizvod != null)
            {
                Db.Proizvodi.Remove(proizvod);
                Db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private void PopuniKategorije(int? izabrana = null)
        {
            ViewBag.Kategorije = new SelectList(Db.Kategorije.OrderBy(k => k.Naziv).ToList(), "Id", "Naziv", izabrana);
        }

        private string SacuvajSliku(HttpPostedFileBase slika)
        {
            if (slika == null || slika.ContentLength == 0)
            {
                return null;
            }

            var dozvoljene = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ekstenzija = Path.GetExtension(slika.FileName ?? string.Empty).ToLowerInvariant();
            if (!dozvoljene.Contains(ekstenzija))
            {
                ModelState.AddModelError("Slika", "Dozvoljene su samo slike (jpg, png, webp, gif).");
                return null;
            }

            if (slika.ContentLength > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("Slika", "Slika ne sme biti veća od 5 MB.");
                return null;
            }

            var folder = Server.MapPath("~/Images/proizvodi");
            Directory.CreateDirectory(folder);

            var ime = Guid.NewGuid().ToString("N") + ekstenzija;
            slika.SaveAs(Path.Combine(folder, ime));

            return "/Images/proizvodi/" + ime;
        }
    }
}
