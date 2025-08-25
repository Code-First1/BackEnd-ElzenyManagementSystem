using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class InventoryProductConfiguration : IEntityTypeConfiguration<InventoryProduct>
    {
        public void Configure(EntityTypeBuilder<InventoryProduct> builder)
        {
            //Validations
            builder
                .Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.MinimumQuantity)
                .HasDefaultValue(0);

            //Realations
            builder
                .HasOne(ip => ip.Product)
                .WithMany(p => p.InventoryProducts)
                .HasForeignKey(ip => ip.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
