using System.ComponentModel.DataAnnotations;

namespace generalStore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = "Please enter the Email!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Phone number / Email")]
        public string UserName { get; set; }

        [Display(Name = "Password")]        
        [Required(ErrorMessage = "Please enter The Password!")]
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters!")]
        public string Password { get; set; }
    }
}
