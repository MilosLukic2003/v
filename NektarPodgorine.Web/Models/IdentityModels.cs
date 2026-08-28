using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NektarPodgorine.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<KategorijaProizvoda> Kategorije { get; set; }

        public DbSet<Proizvod> Proizvodi { get; set; }

        public DbSet<Pcelinjak> Pcelinjaci { get; set; }

        public DbSet<Recenzija> Recenzije { get; set; }

        public DbSet<Vest> Vesti { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            modelBuilder.Entity<Proizvod>()
                .Property(p => p.Cena)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Proizvod>()
                .HasRequired(p => p.Kategorija)
                .WithMany(k => k.Proizvodi)
                .HasForeignKey(p => p.KategorijaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Proizvod>()
                .HasOptional(p => p.Kreirao)
                .WithMany()
                .HasForeignKey(p => p.KreiraoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Recenzija>()
                .HasRequired(r => r.Proizvod)
                .WithMany(p => p.Recenzije)
                .HasForeignKey(r => r.ProizvodId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Recenzija>()
                .HasRequired(r => r.Korisnik)
                .WithMany()
                .HasForeignKey(r => r.KorisnikId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vest>()
                .HasOptional(v => v.Autor)
                .WithMany()
                .HasForeignKey(v => v.AutorId)
                .WillCascadeOnDelete(false);
        }
    }
}
