namespace NektarPodgorine.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NektarPodgorine.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NektarPodgorine.Web.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            foreach (var rola in new[] { "Admin", "User" })
            {
                if (!roleManager.RoleExists(rola))
                {
                    roleManager.Create(new IdentityRole(rola));
                }
            }

            const string adminEmail = "admin@nektarpodgorine.rs";
            var admin = userManager.FindByName(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Administrator gazdinstva",
                    PhoneNumber = "+38269000000",
                    CreatedAt = DateTime.Now
                };
                userManager.Create(admin, "Admin123!");
            }

            if (admin != null && !userManager.IsInRole(admin.Id, "Admin"))
            {
                userManager.AddToRole(admin.Id, "Admin");
            }

            context.SaveChanges();

            context.Kategorije.AddOrUpdate(
                k => k.Naziv,
                new KategorijaProizvoda { Naziv = "Med", Opis = "Razne vrste meda sa pčelinjaka gazdinstva." },
                new KategorijaProizvoda { Naziv = "Pčelinji proizvodi", Opis = "Propolis, polen, matičnji mleč, vosak." },
                new KategorijaProizvoda { Naziv = "Oprema", Opis = "Pčelarska oprema i pribor." });

            context.SaveChanges();

            var med = context.Kategorije.First(k => k.Naziv == "Med");
            var pcelinjiProizvodi = context.Kategorije.First(k => k.Naziv == "Pčelinji proizvodi");

            context.Proizvodi.AddOrUpdate(
                p => p.Naziv,
                new Proizvod
                {
                    Naziv = "Bagremov med",
                    Opis = "Svetao, blagog ukusa, sporo kristališe. Sa bagremovih paša Podgorine.",
                    Cena = 1400m,
                    JedinicaMere = "kg",
                    KolicinaNaStanju = 40,
                    KategorijaId = med.Id,
                    DatumDodavanja = DateTime.Now,
                    KreiraoId = admin != null ? admin.Id : null
                },
                new Proizvod
                {
                    Naziv = "Livadski med",
                    Opis = "Med sa livadskih paša, izražene arome, bogat mineralima.",
                    Cena = 1100m,
                    JedinicaMere = "kg",
                    KolicinaNaStanju = 60,
                    KategorijaId = med.Id,
                    DatumDodavanja = DateTime.Now,
                    KreiraoId = admin != null ? admin.Id : null
                },
                new Proizvod
                {
                    Naziv = "Propolis kapi",
                    Opis = "Alkoholni rastvor propolisa, 30 ml, za jačanje imuniteta.",
                    Cena = 600m,
                    JedinicaMere = "ml",
                    KolicinaNaStanju = 100,
                    KategorijaId = pcelinjiProizvodi.Id,
                    DatumDodavanja = DateTime.Now,
                    KreiraoId = admin != null ? admin.Id : null
                },
                new Proizvod
                {
                    Naziv = "Matičnji mleč",
                    Opis = "Svež matičnji mleč, 10 g, čuva se u frižideru.",
                    Cena = 1800m,
                    JedinicaMere = "g",
                    KolicinaNaStanju = 25,
                    KategorijaId = pcelinjiProizvodi.Id,
                    DatumDodavanja = DateTime.Now,
                    KreiraoId = admin != null ? admin.Id : null
                });

            context.Pcelinjaci.AddOrUpdate(
                p => p.Naziv,
                new Pcelinjak
                {
                    Naziv = "Pčelinjak Podgorina",
                    Mesto = "Podgorina",
                    Opis = "Glavni pčelinjak gazdinstva Nektar Podgorine.",
                    GeografskaSirina = 44.2833,
                    GeografskaDuzina = 19.6833
                });

            context.SaveChanges();
        }
    }
}
