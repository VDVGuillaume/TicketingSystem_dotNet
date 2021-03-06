using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CreateContractCommand : BaseCommand<Contract>
    {
        public ContractType Type { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Client Client { get; set; }

    }
}
