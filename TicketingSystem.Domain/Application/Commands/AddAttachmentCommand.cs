using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class AddAttachmentCommand : BaseCommand<Ticket>
    {
        public List<IFormFile> Attachments { get; set; }
    }
}