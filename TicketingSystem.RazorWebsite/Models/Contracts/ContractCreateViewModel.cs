using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models.Contracts
{
    public class ContractCreateViewModel : IValidatableObject
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> ContractTypes { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Startdatum is verplicht.")]
            [DataType(DataType.Date), DisplayFormat(DataFormatString = "0:dd/MM/yyyy", ApplyFormatInEditMode = true)]
            [Display(Name = "Startdatum")]
            public DateTime StartDate { get; set; }
            [Required(ErrorMessage = "Einddatum is verplicht.")]
            [DataType(DataType.Date), DisplayFormat(DataFormatString = "0:dd/MM/yyyy", ApplyFormatInEditMode = true)]
            [Display(Name = "Einddatum")]
            public DateTime EndDate { get; set; }
            [Required(ErrorMessage = "Type is verplicht.")]
            [Display(Name = "Type")]
            public string Type { get; set; }
            [Display(Name = "Klant")]
            public string ClientName { get; set; }
        }
        
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (Input.StartDate.Date < DateTime.Today.Date) 
            {
                yield return new ValidationResult("Startdatum mag niet in het verleden staan.", new List<string> {"Input.StartDate"});
            }

            if (Input.EndDate.Date <= DateTime.Today.Date) 
            { 
                yield return new ValidationResult("Einddatum moet later zijn dan vandaag", new List<string> {"Input.EndDate"});
            }

            if (Input.EndDate.Date <= Input.StartDate.Date)
            {
                yield return new ValidationResult("Einddatum moet groter zijn dan de startdatum", new List<string> { "Input.EndDate" });
            }
        }
    }
}