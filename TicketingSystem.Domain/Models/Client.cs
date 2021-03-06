using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Contract> Contracts { get; set; }

        public Client(string name)
        {
            this.Name = name;
        }

        private Client()
        {

        }
    }
}
