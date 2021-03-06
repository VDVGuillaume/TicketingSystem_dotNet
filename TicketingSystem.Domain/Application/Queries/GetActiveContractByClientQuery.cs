using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetActiveContractByClientQuery : BaseQuery<Contract>
    {
        public Client Client { get; set; }
    }
}
