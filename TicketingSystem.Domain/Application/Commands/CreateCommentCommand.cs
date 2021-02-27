using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CreateCommentCommand : BaseCommand<Comment>
    {
        public int TicketNr;
        public string Text;
        public DateTime DateAdded;
        public IdentityUser Client;
    }
}
