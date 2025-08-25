using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ShopProduct
{
    public class ShopProductCreateDto
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public int Quantity { get; set; }
        public int SmallBoxesPerBigBox { get; set; }
        public int FullBigBoxesCount { get; set; }
        public int OpenedBigBoxRemaining { get; set; }
        public int OpenedRollRemaining { get; set; }
    }
}
