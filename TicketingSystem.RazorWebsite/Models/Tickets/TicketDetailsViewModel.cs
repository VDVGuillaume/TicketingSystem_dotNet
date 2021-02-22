using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketDetailsViewModel
    {
        public TicketDetailsDTO Ticket { get; set; }
    }

    public class TicketDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public IdentityUser Client { get; set; }
        public List<Comment> Comments { get; set; }
    }
}