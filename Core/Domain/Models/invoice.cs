using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Invoice : BaseEntity<int>
    {
        [Required(ErrorMessage = "Invoice number is required.")]
        [StringLength(50, ErrorMessage = "Invoice number must be less than 50 characters.")]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = "Invoice date is required.")]
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Seller name is required.")]
        [StringLength(150, ErrorMessage = "Seller name must be less than 150 characters.")]
        public string SellerName { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        public decimal TotalAmount { get; set; }

        // Navigation Property 
        public ICollection<InvoiceProduct> InvoiceProducts { get; set; }
    }
}
