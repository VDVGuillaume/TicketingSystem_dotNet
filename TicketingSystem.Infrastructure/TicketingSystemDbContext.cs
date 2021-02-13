using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure
{
    public class TicketingSystemDbContext : IdentityDbContext<ApplicationUser>
    {

        // public DbSet<Model> Models {get;set;}
        public TicketingSystemDbContext(DbContextOptions<TicketingSystemDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.ApplyConfiguration(new ModelEntityTypeConfiguration());
        }
    }
}
