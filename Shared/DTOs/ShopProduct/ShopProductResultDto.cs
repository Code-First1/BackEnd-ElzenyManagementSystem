using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ShopProduct
{
    public class ShopProductResultDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int SmallBoxesPerBigBox { get; set; }
        public int FullBigBoxesCount { get; set; }
        public int OpenedBigBoxRemaining { get; set; }
        public int OpenedRollRemaining { get; set; }

        public ProductResultDto Product {  get; set; }
    }
}
