using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.EntityConfigurations
{
    class ContractEntityTypeConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contracts");

            builder.HasKey(t => t.ContractId);

            builder.HasOne(t => t.Type)
                .WithMany()
                .IsRequired(true);

            builder.HasOne(t => t.Client)
                .WithMany(t => t.Contracts);

            builder.HasMany(t => t.Tickets)
                .WithOne(t => t.Contract);
        }
    }
}
