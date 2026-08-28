using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models
{
    public class Proizvod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv proizvoda je obavezan.")]
        [StringLength(150)]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Cena mora biti veća od 0.")]
        [Display(Name = "Cena")]
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Jedinica mere je obavezna.")]
        [StringLength(20)]
        [Display(Name = "Jedinica mere")]
        public string JedinicaMere { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Količina ne može biti negativna.")]
        [Display(Name = "Količina na stanju")]
        public int KolicinaNaStanju { get; set; }

        [StringLength(500)]
        [Display(Name = "Slika (URL)")]
        public string ImageUrl { get; set; }

        [Display(Name = "Kategorija")]
        public int KategorijaId { get; set; }

        [Display(Name = "Datum dodavanja")]
        public DateTime DatumDodavanja { get; set; }

        public string KreiraoId { get; set; }

        public virtual KategorijaProizvoda Kategorija { get; set; }

        public virtual ApplicationUser Kreirao { get; set; }

        public virtual ICollection<Recenzija> Recenzije { get; set; }
    }
}
