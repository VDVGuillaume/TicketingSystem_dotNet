using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.EntityConfigurations
{
    class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<Attachment>
    {

        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments");

            builder.HasKey(t => t.AttachmentId);
        }
    }
}
