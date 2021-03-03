using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models.Contracts
{
    public class ContractIndexViewModel
    {
        public List<ContractBaseInfoViewModel> Contracts { get; set; }
        public FilterInputModel FilterInput { get; set; }
    }

    public class FilterInputModel
    {
        [Display(Name = "Lopend")]
        public bool FilterStatusActive { get; set; }
        [Display(Name = "In Aanvraag")]
        public bool FilterStatusPending { get; set; }
        [Display(Name = "Afgehandeld")]
        public bool FilterStatusClosed { get; set; }
    }
}
