using TicketingSystem.Domain.Models;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetUserByUsernameQuery : BaseQuery<ApplicationUser>
    {
        public string Username { get; set; }
    }
}
