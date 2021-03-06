using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketUpdateViewModel
    {
        public TicketDetailInfoViewModel Ticket { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> TicketTypes { get; set; }

        public List<SelectListItem> EngineerUsers { get; set; }

        public class InputModel
        {
            public string Title { get; set; }
            [Required(ErrorMessage ="Omschrijving is verplicht.")]
            public string Description { get; set; }
            public string Type { get; set; }
            public string AssignedEngineer { get; set; }

            [Display(Name = "Bijlage")]
            public List<IFormFile> Attachments { get; set; }
        }
    }
}
