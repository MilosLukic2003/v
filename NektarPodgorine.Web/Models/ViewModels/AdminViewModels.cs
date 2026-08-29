using System;
using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class AdminKorisnikVM
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool JeAdmin { get; set; }
    }

    public class KategorijaVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan.")]
        [StringLength(100)]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }

        [StringLength(500)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }
    }
}
