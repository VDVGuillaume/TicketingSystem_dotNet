using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetClientByNameQuery : BaseQuery<Client>
    {
        public string Name { get; set; }
    }
}
