using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RequiredSLA { get; set; }
    }
}
