using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetTicketByIdQuery : BaseQuery<Ticket>
    {
        public string Id { get; set; }
    }
}
