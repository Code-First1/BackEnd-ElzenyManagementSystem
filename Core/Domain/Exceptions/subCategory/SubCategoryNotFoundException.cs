using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.category
{
    public class SubCategoryNotFoundException(int id) :
        NotFoundException(message: $"SubCategory With Id {id} Not Found !!")
    {
    }
}
