using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketsIndexViewModel
    {
        public List<TicketIndexDTO> Tickets { get; set; }
    }

    public class TicketIndexDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
    }
}
