using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class VestCreateVM
    {
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
    }

    public class VestEditVM : VestCreateVM
    {
        public int Id { get; set; }
    }
}
