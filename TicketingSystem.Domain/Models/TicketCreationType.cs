using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{
    /*
    public enum TicketCreationTypeName
    {
        Email,
        Telefonisch,
        Applicatie
    }
    */

    public class TicketCreationType
    {
        public int TicketCreationTypeId { get; set; }
        public string Name { get; set; }
        public List<ContractType> ContractTypes { get; set; }

        public TicketCreationType(string name)
        {
            this.Name = name;
            this.ContractTypes = new List<ContractType>();
        }
    }

}
