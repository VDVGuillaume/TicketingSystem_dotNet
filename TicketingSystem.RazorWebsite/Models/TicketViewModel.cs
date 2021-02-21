using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using TicketingSystem.Common.Enums;

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
        }
    }
}
