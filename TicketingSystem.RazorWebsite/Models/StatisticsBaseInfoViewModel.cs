using System;

namespace TicketingSystem.RazorWebsite.Models
{
    public class StatisticsBaseInfoViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
        public string AverageSolutionTime { get; set; }
    }
}
