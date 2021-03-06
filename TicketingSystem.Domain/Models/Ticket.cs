using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public TicketStatus Status { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        [Required]
        public Client Client { get; set; }
        public ApplicationUser AssignedEngineer { get; set; }
        [Required]
        public TicketType Type { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Attachment> Attachments { get; set; }
        [Required]
        public Contract Contract { get; set; }

        public Ticket(string title, string description, TicketType type, Client client, Contract contract)
        {
            this.Title = title;
            this.Description = description;
            this.Type = type;
            this.DateAdded = DateTime.Now;
            this.Client = client ?? throw new ArgumentNullException();
            Comments = new List<Comment>();
            Attachments = new List<Attachment>();
            Contract = contract ?? throw new ArgumentNullException();
        }

        private Ticket()
        {

        }
    }
}