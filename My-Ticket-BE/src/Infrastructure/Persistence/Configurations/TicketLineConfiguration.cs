using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    class TicketLineConfiguration : IEntityTypeConfiguration<TicketLine>
    {
        public void Configure(EntityTypeBuilder<TicketLine> builder)
        {
            builder.Property(t => t.Email)
                .HasMaxLength(255);
        }
    }
}
