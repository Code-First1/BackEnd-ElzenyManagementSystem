using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Invoice : BaseEntity<int>
    {

        [Required(ErrorMessage = "Invoice date is required.")]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Total amount is required.")]
        public decimal TotalPrice { get; set; }

        //Forign Keys
        public int ShopId { get; set; }
        public string UserId { get; set; }


        // Navigation Property 
        public ICollection<InvoiceProduct> InvoiceProducts { get; set; }
        public Shop Shop { get; set; }
        public AppUser User { get; set; }


    }
}
