using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.ShopProduct
{
    public class ShopProductNotFoundException(int id) :
        NotFoundException(message: $"ShopProduct With Id {id} Not Found !!")
    {
    }
}
