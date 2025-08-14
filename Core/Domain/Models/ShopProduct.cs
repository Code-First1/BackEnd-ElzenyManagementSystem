using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ShopProduct : BaseEntity<int>
    {


        public int Quantity { get; set; }

        public int OpenedRollRemaining { get; set; }

        public int SmallBoxesPerBigBox { get; set; }

        public int FullBigBoxesCount { get; set; }

        public int OpenedBigBoxRemaining { get; set; }

        //ForigenKesy
        public int ShopId { get; set; }
        public int ProductId { get; set; }


        //Navigation Properties
        public Shop Shop { get; set; }
        public Product Product { get; set; }


    }
}