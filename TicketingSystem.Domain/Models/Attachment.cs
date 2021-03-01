using System;

namespace TicketingSystem.Domain.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }

        public Attachment(string name)
        {
            Name = name;
        }
    }
}
