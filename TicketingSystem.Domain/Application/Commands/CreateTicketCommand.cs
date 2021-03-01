using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CreateTicketCommand : BaseCommand<Ticket>
    {
        public string Title { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public IdentityUser Client { get; set; }
        public string Username { get; set; }
        public string AssignedEngineer { get; set; }
        public string Type { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
