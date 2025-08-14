using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Shop : BaseEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ShopProduct> ShopProducts { get; set; }
        
    }
}
