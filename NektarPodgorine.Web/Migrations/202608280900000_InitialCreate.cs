namespace NektarPodgorine.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KategorijaProizvoda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 100),
                        Opis = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proizvod",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 150),
                        Opis = c.String(nullable: false),
                        Cena = c.Decimal(nullable: false, precision: 18, scale: 2),
                        JedinicaMere = c.String(nullable: false, maxLength: 20),
                        KolicinaNaStanju = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 500),
                        KategorijaId = c.Int(nullable: false),
                        DatumDodavanja = c.DateTime(nullable: false),
                        KreiraoId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KategorijaProizvoda", t => t.KategorijaId)
                .ForeignKey("dbo.AspNetUsers", t => t.KreiraoId)
                .Index(t => t.KategorijaId)
                .Index(t => t.KreiraoId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Recenzija",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ocena = c.Int(nullable: false),
                        Sadrzaj = c.String(nullable: false, maxLength: 2000),
                        DatumKreiranja = c.DateTime(nullable: false),
                        ProizvodId = c.Int(nullable: false),
                        KorisnikId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.KorisnikId)
                .ForeignKey("dbo.Proizvod", t => t.ProizvodId, cascadeDelete: true)
                .Index(t => t.ProizvodId)
                .Index(t => t.KorisnikId);
            
            CreateTable(
                "dbo.Pcelinjak",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(nullable: false, maxLength: 150),
                        Mesto = c.String(nullable: false, maxLength: 150),
                        Opis = c.String(maxLength: 1000),
                        GeografskaSirina = c.Double(nullable: false),
                        GeografskaDuzina = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Vest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naslov = c.String(nullable: false, maxLength: 200),
                        Sadrzaj = c.String(nullable: false),
                        ImageUrl = c.String(maxLength: 500),
                        DatumObjave = c.DateTime(nullable: false),
                        AutorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AutorId)
                .Index(t => t.AutorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vest", "AutorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Recenzija", "ProizvodId", "dbo.Proizvod");
            DropForeignKey("dbo.Recenzija", "KorisnikId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Proizvod", "KreiraoId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Proizvod", "KategorijaId", "dbo.KategorijaProizvoda");
            DropIndex("dbo.Vest", new[] { "AutorId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Recenzija", new[] { "KorisnikId" });
            DropIndex("dbo.Recenzija", new[] { "ProizvodId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Proizvod", new[] { "KreiraoId" });
            DropIndex("dbo.Proizvod", new[] { "KategorijaId" });
            DropTable("dbo.Vest");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Pcelinjak");
            DropTable("dbo.Recenzija");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Proizvod");
            DropTable("dbo.KategorijaProizvoda");
        }
    }
}
