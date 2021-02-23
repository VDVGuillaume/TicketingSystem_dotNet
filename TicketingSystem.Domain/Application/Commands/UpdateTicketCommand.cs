using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class UpdateTicketCommand : BaseCommand<Ticket>
    {
        public int Ticketnr;
        public string Title;
        public string Description;
        public string AssignedEngineer;
        public string Type;
    }
}
