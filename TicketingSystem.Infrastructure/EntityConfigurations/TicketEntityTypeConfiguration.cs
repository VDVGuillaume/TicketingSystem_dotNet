using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.EntityConfigurations
{
    class TicketEntityTypeConfiguration : IEntityTypeConfiguration<Ticket>
    {

        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");

            builder.HasKey(t => t.Ticketnr);

            builder.HasOne(t => t.Client)
                .WithMany();

            builder.HasOne(t => t.AssignedEngineer)
                .WithMany();

            builder.HasMany(t => t.Comments)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Contract)
                .WithMany(t => t.Tickets)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
