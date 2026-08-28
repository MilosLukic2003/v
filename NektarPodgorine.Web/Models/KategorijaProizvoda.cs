using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models
{
    public class KategorijaProizvoda
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv kategorije je obavezan.")]
        [StringLength(100)]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }

        [StringLength(500)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        public virtual ICollection<Proizvod> Proizvodi { get; set; }
    }
}
