using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{
    public enum TicketCreationTime
    {
        Always,
        Weekdays
    }

    public class ContractType
    {
        public int ContractTypeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<TicketCreationType> TicketCreationTypes { get; set; }
        public TicketCreationTime TicketCreationTime { get; set; }

        public ContractType(string name, bool active, TicketCreationTime ticketCreationTime, List<TicketCreationType> ticketCreationTypes = null)
        {
            this.Name = name;
            this.Active = active;
            this.TicketCreationTime = ticketCreationTime;
            this.TicketCreationTypes = ticketCreationTypes;
        }

        private ContractType()
        {
        }
    }
}
