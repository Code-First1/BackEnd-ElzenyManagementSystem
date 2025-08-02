using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        //Forign Keys
        public int CategoryId { get; set; }


        //Navigation Properties
        public Category Category { get; set; } 
    }
}
