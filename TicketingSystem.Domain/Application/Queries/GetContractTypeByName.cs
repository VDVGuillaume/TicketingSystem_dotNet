using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetContractTypeByNameQuery : BaseQuery<ContractType>
    {
        public string Name { get; set; }
    }
}