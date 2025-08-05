using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.product
{
    public class ProductNotFoundExceptions(int id) : 
        NotFoundException(message: $"Product With Id {id} Not Found !!")
    {
    }
}
