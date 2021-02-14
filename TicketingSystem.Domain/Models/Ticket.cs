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
        public int Ticketnr;
        public string Title;
        public Status Status;
        public DateTime DateAdded;
        public string Description;
        public Client Client;
        public string AssignedEngineer;
        public string Type;
        public List<string> Comments;
        public List<Attachment> Attachments;


        public Ticket(string title, string description, string  type, Attachment attachment = null)
        {
            this.Title = title;
            this.Description = description;
            this.Type = type;
            this.DateAdded = DateTime.Now;
            Attachments.Add(attachment);

        }








    }




}
