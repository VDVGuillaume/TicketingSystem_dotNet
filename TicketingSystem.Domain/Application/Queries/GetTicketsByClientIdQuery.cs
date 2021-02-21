using System.Linq;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetTicketsByClientIdQuery : BaseQuery<IQueryable<Ticket>>
    {
        public string ClientId { get; set; }
    }
}
