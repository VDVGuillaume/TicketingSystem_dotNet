using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetContractByIdQuery : BaseQuery<Contract>
    {
        public int Id { get; set; }
    }
}
