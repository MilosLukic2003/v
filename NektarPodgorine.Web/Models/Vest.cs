using System;
using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models
{
    public class Vest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov je obavezan.")]
        [StringLength(200)]
        [Display(Name = "Naslov")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Sadržaj je obavezan.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Sadržaj")]
        public string Sadrzaj { get; set; }

        [StringLength(500)]
        [Display(Name = "Slika (URL)")]
        public string ImageUrl { get; set; }

        [Display(Name = "Datum objave")]
        public DateTime DatumObjave { get; set; }

        public string AutorId { get; set; }

        public virtual ApplicationUser Autor { get; set; }
    }
}
