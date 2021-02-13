using Microsoft.AspNetCore.Identity;

namespace TicketingSystem.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Blocked { get; set; }
    }
}
