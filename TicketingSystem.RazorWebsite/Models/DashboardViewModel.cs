using System.Collections.Generic;
using TicketingSystem.RazorWebsite.Models.Contracts;
using TicketingSystem.RazorWebsite.Models.Tickets;

namespace TicketingSystem.RazorWebsite.Models
{
    public class DashboardViewModel
    {
        public List<TicketBaseInfoViewModel> OpenTickets { get; set; }
        public List<TicketBaseInfoViewModel> ClosedTickets { get; set; }
        public List<ContractBaseInfoViewModel> ActiveContracts { get; set; }
    }
}
