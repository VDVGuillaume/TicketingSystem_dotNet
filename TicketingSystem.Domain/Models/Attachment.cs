using System;

namespace TicketingSystem.Domain.Models
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Attachment(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
