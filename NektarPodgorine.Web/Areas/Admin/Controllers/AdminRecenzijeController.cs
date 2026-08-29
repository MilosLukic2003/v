using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Models;

namespace NektarPodgorine.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminRecenzijeController : Controller
    {
        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public ActionResult Index()
        {
            var recenzije = Db.Recenzije
                .Include(r => r.Proizvod)
                .Include(r => r.Korisnik)
                .OrderByDescending(r => r.DatumKreiranja)
                .ToList();

            return View(recenzije);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Obrisi(int id)
        {
            var recenzija = Db.Recenzije.Find(id);
            if (recenzija != null)
            {
                Db.Recenzije.Remove(recenzija);
                Db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
