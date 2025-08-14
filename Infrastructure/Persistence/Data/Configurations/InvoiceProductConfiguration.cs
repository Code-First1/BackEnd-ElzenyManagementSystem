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
    public class InvoiceProductConfiguration : IEntityTypeConfiguration<InvoiceProduct>
    {
        public void Configure(EntityTypeBuilder<InvoiceProduct> builder)
        {
            builder
                .Property(e => e.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceProducts)
                .HasForeignKey(ii => ii.InvoiceId);

            builder
                .HasOne(ii => ii.Product)
                .WithMany(p => p.InvoiceProducts)
                .HasForeignKey(ii => ii.ProductId);
        }
    }
}
