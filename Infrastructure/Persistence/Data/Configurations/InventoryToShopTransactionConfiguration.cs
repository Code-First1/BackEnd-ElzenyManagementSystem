using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class InventoryToShopTransactionConfiguration : IEntityTypeConfiguration<InventoryToShopTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryToShopTransaction> builder)
        {
            builder.HasMany(t => t.Items)
                   .WithOne(i => i.Transaction)
                   .HasForeignKey(i => i.TransactionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
