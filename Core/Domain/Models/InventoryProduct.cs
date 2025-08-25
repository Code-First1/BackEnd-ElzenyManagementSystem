using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class InventoryProduct : BaseEntity<int>
    {
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; } = 0;

        //Forign Keys
        public int ProductId { get; set; }

        //Navigations
        public Product Product { get; set; }


    }
}
