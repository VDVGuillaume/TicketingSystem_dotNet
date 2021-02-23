using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketIndexViewModel
    {
        public List<TicketIndexDTO> Tickets { get; set; }
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

    public class TicketIndexDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }
}
