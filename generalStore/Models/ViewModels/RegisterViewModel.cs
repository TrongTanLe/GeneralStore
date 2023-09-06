using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace generalStore.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter the full name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter the email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string Email { get; set; }
        [MaxLength(11)]
        [Required(ErrorMessage = "Please enter the phone number")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string Phone { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter the password")]
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters")]
        public string Password { get; set; }
        [MinLength(5, ErrorMessage = "You need to set a password of at least 5 characters")]
        [Display(Name = "Enter the password")]
        [Compare("Password", ErrorMessage = "Please enter the same password")]
        public string ConfirmPassword { get; set; }
    }
}
