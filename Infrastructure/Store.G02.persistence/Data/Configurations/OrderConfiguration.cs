using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.G02.Domain.Entities.orders;
using Store.G02.persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress);

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(O => O.DeliveryMethodId);

            builder.HasMany(O => O.Items)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            builder.Property(O => O.SubTotal)
                  .HasColumnType("decimal(18,2)"); // Configure SubTotal precision
        }
    }
}
