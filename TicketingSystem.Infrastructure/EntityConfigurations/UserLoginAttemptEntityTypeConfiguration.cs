using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.EntityConfigurations
{
    public class UserLoginAttemptEntityTypeConfiguration : IEntityTypeConfiguration<UserLoginAttempt>
    {
        public void Configure(EntityTypeBuilder<UserLoginAttempt> builder)
        {
            builder.ToTable("UserLoginAttempts");

            builder.HasKey(t => t.Id);
        }
    }
}
