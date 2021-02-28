using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.EntityConfigurations
{
    class ContractTypeEntityTypeConfiguration : IEntityTypeConfiguration<ContractType>
    {
        public void Configure(EntityTypeBuilder<ContractType> builder)
        {
            builder.ToTable("ContractTypes");

            builder.HasKey(t => t.ContractTypeId);

            builder.HasMany(t => t.TicketCreationTypes)
                .WithMany(t => t.ContractTypes);
        }
    }
}
