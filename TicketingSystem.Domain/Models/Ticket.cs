using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TicketingSystem.Domain.Models
{

    public enum TicketStatus
    {
        Aangemaakt,
        InBehandeling,
        Afgehandeld,
        Geannuleerd
    }

    public class Ticket
    {
        public int Ticketnr { get; }
        public string Title { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public Client Client { get; set; }
        public string AssignedEngineer { get; set; }
        public TicketType Type { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Attachment> Attachments { get; set; }

        public Ticket(string title, string description, TicketType type, Client client)
        {
            this.Title = title;
            this.Description = description;
            this.Type = type;
            this.DateAdded = DateTime.Now;
            this.Client = client;
            Comments = new List<Comment>();
            Attachments = new List<Attachment>();
        }

        private Ticket()
        {

        }
    }
}