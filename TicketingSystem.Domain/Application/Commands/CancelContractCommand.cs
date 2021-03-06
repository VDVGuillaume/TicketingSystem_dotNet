using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Commands
{
    public class CancelContractCommand : BaseCommand<Contract>
    {
        public int ContractId { get; set; }
    }
}
