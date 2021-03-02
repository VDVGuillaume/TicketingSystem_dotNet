using System;

namespace TicketingSystem.Domain.Models
{
    public class Comment
    {
        public int CommentId { get; set; }        
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}
