using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.category
{
    public class CategoryNotFoundException(int id) :
        NotFoundException(message: $"Category With Id {id} Not Found !!")
    {
    }
}
