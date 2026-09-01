using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Infrastructure;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Models.ViewModels;

namespace NektarPodgorine.Web.Areas.Admin.Controllers
{
    [Autorizacija(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public ActionResult Index()
        {
            var model = Db.Users
                .OrderBy(u => u.Email)
                .ToList()
                .Select(u => new AdminKorisnikVM
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    CreatedAt = u.CreatedAt,
                    JeAdmin = UserManager.IsInRole(u.Id, "Admin")
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromeniRolu(string id)
        {
            if (id == User.Identity.GetUserId())
            {
                TempData["Greska"] = "Ne možete menjati sopstvenu rolu.";
                return RedirectToAction("Index");
            }

            var korisnik = UserManager.FindById(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            if (UserManager.IsInRole(id, "Admin"))
            {
                UserManager.RemoveFromRole(id, "Admin");
                UserManager.AddToRole(id, "User");
            }
            else
            {
                UserManager.RemoveFromRole(id, "User");
                UserManager.AddToRole(id, "Admin");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Obrisi(string id)
        {
            if (id == User.Identity.GetUserId())
            {
                TempData["Greska"] = "Ne možete obrisati sopstveni nalog.";
                return RedirectToAction("Index");
            }

            var korisnik = UserManager.FindById(id);
            if (korisnik == null)
            {
                return HttpNotFound();
            }

            Db.Recenzije.RemoveRange(Db.Recenzije.Where(r => r.KorisnikId == id));
            foreach (var p in Db.Proizvodi.Where(p => p.KreiraoId == id))
            {
                p.KreiraoId = null;
            }
            foreach (var v in Db.Vesti.Where(v => v.AutorId == id))
            {
                v.AutorId = null;
            }
            Db.SaveChanges();

            UserManager.Delete(korisnik);

            return RedirectToAction("Index");
        }
    }
}
