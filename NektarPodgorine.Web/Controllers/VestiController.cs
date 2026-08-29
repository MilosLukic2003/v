using System;
using System.Data.Entity;
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
    public class VestiController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public ActionResult Index()
        {
            var vesti = Db.Vesti
                .Include(v => v.Autor)
                .OrderByDescending(v => v.DatumObjave)
                .ToList();

            return View(vesti);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vest = Db.Vesti.Include(v => v.Autor).SingleOrDefault(v => v.Id == id.Value);
            if (vest == null)
            {
                return HttpNotFound();
            }

            return View(vest);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new VestCreateVM());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VestCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Db.Vesti.Add(new Vest
            {
                Naslov = model.Naslov,
                Sadrzaj = model.Sadrzaj,
                ImageUrl = model.ImageUrl,
                DatumObjave = DateTime.Now,
                AutorId = User.Identity.GetUserId()
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

            var vest = Db.Vesti.Find(id.Value);
            if (vest == null)
            {
                return HttpNotFound();
            }

            return View(new VestEditVM
            {
                Id = vest.Id,
                Naslov = vest.Naslov,
                Sadrzaj = vest.Sadrzaj,
                ImageUrl = vest.ImageUrl
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VestEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var vest = Db.Vesti.Find(model.Id);
            if (vest == null)
            {
                return HttpNotFound();
            }

            vest.Naslov = model.Naslov;
            vest.Sadrzaj = model.Sadrzaj;
            vest.ImageUrl = model.ImageUrl;
            Db.SaveChanges();

            return RedirectToAction("Details", new { id = vest.Id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vest = Db.Vesti.Include(v => v.Autor).SingleOrDefault(v => v.Id == id.Value);
            if (vest == null)
            {
                return HttpNotFound();
            }

            return View(vest);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var vest = Db.Vesti.Find(id);
            if (vest != null)
            {
                Db.Vesti.Remove(vest);
                Db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
