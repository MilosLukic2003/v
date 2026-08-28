using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NektarPodgorine.Web.Models;
using NektarPodgorine.Web.Models.ViewModels;

namespace NektarPodgorine.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        private ApplicationDbContext Db
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            var model = new ManageIndexViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                HasPassword = await UserManager.HasPasswordAsync(userId),
                CreatedAt = user.CreatedAt
            };

            return View(model);
        }

        public async Task<ActionResult> EditProfile()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return View(new EditProfileViewModel
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            await UserManager.UpdateAsync(user);

            TempData["Status"] = "Profil je sačuvan.";
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Status"] = "Lozinka je promenjena.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(model);
        }

        public ActionResult MojeRecenzije()
        {
            var userId = User.Identity.GetUserId();
            var recenzije = Db.Recenzije
                .Include(r => r.Proizvod)
                .Where(r => r.KorisnikId == userId)
                .OrderByDescending(r => r.DatumKreiranja)
                .ToList();

            return View(recenzije);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}
