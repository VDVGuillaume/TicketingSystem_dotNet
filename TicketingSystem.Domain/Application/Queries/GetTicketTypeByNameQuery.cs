using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetTicketTypeByNameQuery : BaseQuery<TicketType>
    {
        public string Name { get; set; }
    }
}
