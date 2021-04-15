using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    class TicketHeaderConfiguration : IEntityTypeConfiguration<TicketHeader>
    {
        public void Configure(EntityTypeBuilder<TicketHeader> builder)
        {
            builder.Property(t => t.Title)
                .HasMaxLength(200);
        }
    }
}
