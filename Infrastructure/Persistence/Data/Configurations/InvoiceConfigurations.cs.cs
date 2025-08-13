using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class InvoiceConfigurations : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            
            builder.ToTable("Invoices");

            
            builder.HasKey(i => i.Id);

            
            builder.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.SellerName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // one to many
            builder.HasMany(i => i.InvoiceProducts)
                   .WithOne(ip => ip.Invoice)
                   .HasForeignKey(ip => ip.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
