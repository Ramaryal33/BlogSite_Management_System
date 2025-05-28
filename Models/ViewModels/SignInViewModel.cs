using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MurkyPluse.Models.ViewModels
{
    public class SignInViewModel
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}

