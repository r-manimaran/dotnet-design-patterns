using eCommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
       builder.HasKey(p=>p.Id);

        builder.Property(p => p.Name)
             .IsRequired()
             .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(255);

        builder.Property(p => p.Price)
              .IsRequired()
              .HasPrecision(18, 2);

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}
