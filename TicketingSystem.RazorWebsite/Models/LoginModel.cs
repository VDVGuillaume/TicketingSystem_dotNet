using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models
{
    public class LoginModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Gebruikersnaam")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Paswoord")]
            public string Password { get; set; }

            [Display(Name = "Login onthouden?")]
            public bool RememberMe { get; set; }
        }
    }
}