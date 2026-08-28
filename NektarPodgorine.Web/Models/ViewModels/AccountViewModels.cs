using System.ComponentModel.DataAnnotations;

namespace NektarPodgorine.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite ispravan email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ime i prezime je obavezno.")]
        [StringLength(150)]
        [Display(Name = "Ime i prezime")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite ispravan email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Unesite ispravan broj telefona.")]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Lozinka mora imati najmanje {2} karaktera.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("Password", ErrorMessage = "Lozinka i potvrda se ne poklapaju.")]
        public string ConfirmPassword { get; set; }
    }
}
