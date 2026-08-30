using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class PcelinjakCreateVM
    {
        [Required(ErrorMessage = "Naziv je obavezan.")]
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

        [Range(-90, 90, ErrorMessage = "Geografska širina mora biti između -90 i 90.")]
        [Display(Name = "Geografska širina")]
        public double GeografskaSirina { get; set; }

        [Range(-180, 180, ErrorMessage = "Geografska dužina mora biti između -180 i 180.")]
        [Display(Name = "Geografska dužina")]
        public double GeografskaDuzina { get; set; }
    }

    public class PcelinjakEditVM : PcelinjakCreateVM
    {
        public int Id { get; set; }
    }
}
