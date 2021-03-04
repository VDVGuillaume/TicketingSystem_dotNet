using System;

namespace TicketingSystem.Domain.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public string VirtualPath { get; set; }

        public Attachment(string name, string virtualPath)
        {
            Name = name;
            VirtualPath = virtualPath;
        }
    }
}
