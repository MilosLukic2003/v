using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NektarPodgorine.Web.Models
{
    // Prošireni korisnik aplikacije. IdentityUser već sadrži UserName, Email,
    // PasswordHash, PhoneNumber i PhoneNumberConfirmed, pa ovde dodajemo samo
    // dodatna polja specifična za gazdinstvo "Nektar Podgorine".
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Napomena: authenticationType mora da se poklapa sa onim definisanim u CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ovde dodati custom claim-ove po potrebi
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        // DbSet-ovi za domenske entitete (Proizvod, KategorijaProizvoda, Pcelinjak,
        // Recenzija, Vest ...) dodaju se u Fazi 2.

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API konfiguracija relacija dolazi u Fazi 2.
        }
    }
}
