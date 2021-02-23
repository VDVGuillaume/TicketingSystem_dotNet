using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketIndexViewModel
    {
        public List<TicketBaseInfoViewModel> Tickets { get; set; }
        public FilterInputModel FilterInput { get; set; }
    }

    public class FilterInputModel 
    { 
        [Display(Name = "Aangemaakt")]
        public bool FilterStatusCreated { get; set; }
        [Display(Name = "In behandeling")]
        public bool FilterStatusInProgress { get; set; }
        [Display(Name = "Afgehandeld")]
        public bool FilterStatusClosed { get; set; }
        [Display(Name = "Geannuleerd")]
        public bool FilterStatusCancelled { get; set; }
    }
}
