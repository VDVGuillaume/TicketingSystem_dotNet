using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    class CreateCommentCommand : BaseCommand<Comment>
    {
        public string Text;
        public DateTime DateAdded;
        public IdentityUser Client;
    }
}
