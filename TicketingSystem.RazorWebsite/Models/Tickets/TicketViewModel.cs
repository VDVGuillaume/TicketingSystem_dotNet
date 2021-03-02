using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> TicketTypes { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name ="Titel")]
            public string Title { get; set; }
            [Required]
            [Display(Name = "Omschrijving")]
            public string Description { get; set; }
            [Display(Name = "Klant")]
            public string ClientName { get; set; }
            [Required]
            [Display(Name = "Type")]
            public string Type { get; set; }

            [Display(Name = "Bijlage")]
            public List<IFormFile> Attachments { get; set; }
        }
    }
}
