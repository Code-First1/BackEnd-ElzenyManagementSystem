using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Product
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 150 characters.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "WholeSale Unit is required.")]
        public string UnitForWholeSale { get; set; }

        [Required(ErrorMessage = "Retail Unit is required.")]
        public string UnitForRetail { get; set; }

        [Required(ErrorMessage = "Price per unit is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per unit must be greater than 0.")]
        public decimal PrieceForWholeSale { get; set; }

        [Required(ErrorMessage = "Price per unit is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per unit must be greater than 0.")]
        public decimal PriceForRetail { get; set; }
        public int QuantityForOrigin { get; set; }

        public string? PictureUrl { get; set; }

        public int CategoryId { get; set; }

        public int? SubCategoryId { get; set; }
    }
}
