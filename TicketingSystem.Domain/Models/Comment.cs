using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TicketingSystem.Domain.Models
{
    public class Comment
    {
        public int CommentId { get; set; }        
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public IdentityUser CreatedBy { get; set; }
    }
}
