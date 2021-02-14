using System.Linq;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetUserLoginAttemptsByUsernameQuery : BaseQuery<IQueryable<UserLoginAttempt>>
    {
        public int? AmountToFetch { get; set; }
        public string Username { get; set; }
    }
}
