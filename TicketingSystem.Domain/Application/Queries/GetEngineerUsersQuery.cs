using System.Linq;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetEngineerUsersQuery : BaseQuery<IQueryable<ApplicationUser>>
    {
    }
}
