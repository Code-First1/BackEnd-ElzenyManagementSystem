using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.TransationFromInventoryToShopInProduct
{
    public class TransactionPerProductResultDto
    {
        public string productName {  get; set; }
        public int QuantityChanged { get; set; }

        public int InventoryQuntityAfter { get; set; }
        public int ShopQuantityAfter { get; set; }
    }
}
