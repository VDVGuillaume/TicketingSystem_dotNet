using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.RazorWebsite.Models.Tickets
{
    public class TicketDetailsViewModel
    {
        public TicketDetailInfoViewModel Ticket { get; set; }
       
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

    }

    public class InputModel
    {
        public string Comment { get; set; }
     
    }

    public class TicketDetailInfoViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
    }

    public class AttachmentViewModel 
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}