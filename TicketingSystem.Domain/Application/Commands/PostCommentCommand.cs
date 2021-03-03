using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class PostCommentCommand : BaseCommand<Comment>
    {
        public int Ticketnr;
        public string Text;
        public DateTime DateAdded;
        public ApplicationUser CreatedBy;
    }
}
