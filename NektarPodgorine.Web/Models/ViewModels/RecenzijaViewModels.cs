using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class RecenzijaCreateVM
    {
        public int ProizvodId { get; set; }

        [Range(1, 5, ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena")]
        public int Ocena { get; set; }

        [Required(ErrorMessage = "Unesite tekst recenzije.")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Komentar")]
        public string Sadrzaj { get; set; }
    }

    public class RecenzijaEditVM : RecenzijaCreateVM
    {
        public int Id { get; set; }

        public string ProizvodNaziv { get; set; }
    }
}
