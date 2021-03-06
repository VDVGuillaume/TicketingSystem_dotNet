using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetActiveContractByClientIdQuery :  BaseQuery<IQueryable<Contract>>
    {
        public int ClientId { get; set; }
        
    }
}
