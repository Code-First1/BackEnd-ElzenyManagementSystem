using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.InventoryProduct
{
    public class InventoryProductNotFoundException(int id) :
        NotFoundException(message: $"InventoryProduct With Id {id} Not Found !!")
    {
    }
}
