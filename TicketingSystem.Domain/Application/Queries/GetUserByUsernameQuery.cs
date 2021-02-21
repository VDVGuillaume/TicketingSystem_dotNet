using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.Domain.Application.Queries
{
    public class GetUserByUsernameQuery : BaseQuery<IdentityUser>
    {
        public string Username { get; set; }
    }
}
