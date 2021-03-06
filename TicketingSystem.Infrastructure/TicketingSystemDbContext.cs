using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.EntityConfigurations;

namespace TicketingSystem.Infrastructure
{
    public class TicketingSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<TicketCreationType> TicketCreationTypes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public TicketingSystemDbContext(DbContextOptions<TicketingSystemDbContext> options) : base(options)
        {
            
        }

        public TicketingSystemDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserLoginAttemptEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TicketEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContractEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContractTypeEntityTypeConfiguration());
        }
    }
}
