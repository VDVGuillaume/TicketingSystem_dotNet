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
        public Client Client { get; set; }
        public List<Ticket> Tickets { get; set; }

        public Contract(ContractType type, ContractStatus status, DateTime validFrom, DateTime validTo, Client client)
        {
            this.Type = type;
            this.Status = status;
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
            this.Client = client;
        }

        private Contract()
        {
        }
    }
}
