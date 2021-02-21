using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models
{
    public class TicketViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }
            public int ClientId { get; set; }
            [Required]
            public string Type { get; set; }
            public List<IFormFile> Attachments { get; set; }
        }
    }
}
