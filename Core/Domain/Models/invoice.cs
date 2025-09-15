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
        public string UserName { get; set; }

        public  int number { get; set; } = 0;

        // Navigation Property 
        public List<InvoiceProduct> InvoiceProducts { get; set; }
        //public List<Product> Products { get; set; }
        public Shop Shop { get; set; }
       
    }
}
