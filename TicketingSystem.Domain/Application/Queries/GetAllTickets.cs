using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetAllTickets : BaseQuery<IQueryable<Ticket>>
    {
    }
}
