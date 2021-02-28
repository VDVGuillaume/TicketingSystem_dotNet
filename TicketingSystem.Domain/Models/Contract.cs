using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingSystem.Domain.Models
{
    public enum ContractStatus
    {
        InAanvraag,
        Lopend,
        Beëindigd
    }
    public class Contract
    {
        public int ContractId { get; set; }
        public ContractType Type { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public Contract(ContractType type, ContractStatus status, DateTime validFrom, DateTime validTo)
        {
            this.Type = type;
            this.Status = status;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
        }

        private Contract()
        {

        }
    }
}
