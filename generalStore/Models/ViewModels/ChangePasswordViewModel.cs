using System.ComponentModel.DataAnnotations;

namespace generalStore.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        [Display(Name = "Password now")]
        public string PasswordNow { get; set; }
        [Display(Name = "New password")]
        [Required(ErrorMessage = "Please input password")]
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters")]
        public string Password { get; set; }
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters")]
        [Display(Name = "Input again ew password")]
        [Compare("Password", ErrorMessage = "Do not same password")]
        public string ConfirmPassword { get; set; }
    }
}
