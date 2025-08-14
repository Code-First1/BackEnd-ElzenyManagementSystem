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
            builder
                .HasOne(ip => ip.Product)
                .WithMany(p => p.InventoryProducts)
                .HasForeignKey(ip => ip.ProductId);
        }
    }
}
