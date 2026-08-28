using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class ProizvodCreateVM
    {
        [Required(ErrorMessage = "Naziv je obavezan.")]
        [StringLength(150)]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Cena mora biti veća od 0.")]
        [Display(Name = "Cena (RSD)")]
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

        [Range(1, int.MaxValue, ErrorMessage = "Izaberite kategoriju.")]
        [Display(Name = "Kategorija")]
        public int KategorijaId { get; set; }
    }

    public class ProizvodEditVM : ProizvodCreateVM
    {
        public int Id { get; set; }
    }
}
