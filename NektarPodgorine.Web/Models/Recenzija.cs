using System;
using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models
{
    public class Recenzija
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena")]
        public int Ocena { get; set; }

        [Required(ErrorMessage = "Sadržaj recenzije je obavezan.")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Komentar")]
        public string Sadrzaj { get; set; }

        [Display(Name = "Datum kreiranja")]
        public DateTime DatumKreiranja { get; set; }

        public int ProizvodId { get; set; }

        public string KorisnikId { get; set; }

        public virtual Proizvod Proizvod { get; set; }

        public virtual ApplicationUser Korisnik { get; set; }
    }
}
