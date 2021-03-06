using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.RazorWebsite.Models.Contracts
{
    public class ContractCreateViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> ContractTypes { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Startdatum")]
            public DateTime StartDate { get; set; }
            [Required]
            [Display(Name = "Einddatum")]
            public DateTime EndDate { get; set; }
            [Required]
            [Display(Name = "Type")]
            public string Type { get; set; }
            [Display(Name = "Klant")]
            public string ClientName { get; set; }
        }
    }
}