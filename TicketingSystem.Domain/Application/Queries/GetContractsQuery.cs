using System.Linq;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetContractsQuery : BaseQuery<IQueryable<Contract>>
    {
    }
}
