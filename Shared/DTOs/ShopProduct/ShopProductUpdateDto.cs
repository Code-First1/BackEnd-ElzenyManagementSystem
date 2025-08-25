using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ShopProduct
{
    public class ShopProductUpdateDto
    {
        public int Quantity { get; set; }
        public int SmallBoxesPerBigBox { get; set; }
        public int FullBigBoxesCount { get; set; }
        public int OpenedBigBoxRemaining { get; set; }
        public int OpenedRollRemaining { get; set; }
    }
}
