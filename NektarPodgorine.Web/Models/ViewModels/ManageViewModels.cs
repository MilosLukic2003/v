using System;
using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class ManageIndexViewModel
    {
        public string Email { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public bool HasPassword { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Trenutna lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Trenutna lozinka")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nova lozinka je obavezna.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Lozinka mora imati najmanje {2} karaktera.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrda nove lozinke")]
        [Compare("NewPassword", ErrorMessage = "Nova lozinka i potvrda se ne poklapaju.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Ime i prezime je obavezno.")]
        [StringLength(150)]
        [Display(Name = "Ime i prezime")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Unesite ispravan broj telefona.")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
    }
}
