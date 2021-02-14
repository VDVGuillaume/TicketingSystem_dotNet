using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.EntityConfigurations;

namespace TicketingSystem.Infrastructure
{
    public class TicketingSystemDbContext : IdentityDbContext
    {
        public DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public TicketingSystemDbContext(DbContextOptions<TicketingSystemDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserLoginAttemptEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TicketEntityTypeConfiguration());
        }
    }
}
