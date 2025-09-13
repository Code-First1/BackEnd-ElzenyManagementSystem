using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class InvoiceConfigurations : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder
                .Property(e => e.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(i => i.Shop)
                .WithMany(s => s.Invoices)
                .HasForeignKey(i => i.ShopId);



            
        }
    }
}
