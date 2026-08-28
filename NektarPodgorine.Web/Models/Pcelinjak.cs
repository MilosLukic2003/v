using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models
{
    public class Pcelinjak
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv pčelinjaka je obavezan.")]
        [StringLength(150)]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Mesto je obavezno.")]
        [StringLength(150)]
        [Display(Name = "Mesto")]
        public string Mesto { get; set; }

        [StringLength(1000)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Range(-90, 90)]
        [Display(Name = "Geografska širina")]
        public double GeografskaSirina { get; set; }

        [Range(-180, 180)]
        [Display(Name = "Geografska dužina")]
        public double GeografskaDuzina { get; set; }
    }
}
