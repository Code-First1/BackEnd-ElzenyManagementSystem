using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product : BaseEntity<int>
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 150 characters.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Unit is required.")]
        public Unit Unit { get; set; }

        [Required(ErrorMessage = "Price per unit is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per unit must be greater than 0.")]
        public decimal PricePerUnit { get; set; }

        public string? PictureUrl { get; set; }


        //Forign Keys
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }


        //Navigation Properties
        public Category Category { get; set; } 
        public SubCategory SubCategory { get; set; }
        public IEnumerable<InventoryProduct> InventoryProducts { get; set; }
        public IEnumerable<ShopProduct> ShopProducts { get; set; }
        public IEnumerable<InvoiceProduct> InvoiceProducts { get; set; }

    }
}
