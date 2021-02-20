using System;
using System.Collections.Generic;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CreateTicketCommand : BaseCommand<Ticket>
    {
        public string Title;
        public Status Status;
        public DateTime DateAdded;
        public string Description;
        public Client Client;
        public string AssignedEngineer;
        public string Type;
        public List<Comment> Comments;
        public List<Attachment> Attachments;
    }
}
