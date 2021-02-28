using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }

        public Client(string name)
        {
            this.Name = name;
        }

        private Client()
        {

        }
    }
}
