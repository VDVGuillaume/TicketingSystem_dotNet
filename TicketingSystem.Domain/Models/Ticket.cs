using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{

    public enum Status
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
        public Status Status { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public Client Client { get; set; }
        public string AssignedEngineer { get; set; }
        public string Type { get; set; }
        public List<string> Comments { get; set; }
        public List<Attachment> Attachments { get; set; }


        public Ticket(string title, string description, string  type, List<Attachment> attachments = null)
        {
            this.Title = title;
            this.Description = description;
            this.Type = type;
            this.DateAdded = DateTime.Now;
            this.Attachments = attachments;

        }

        private Ticket()
        {

        }



    }




}
