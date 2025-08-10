using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Auth
{
    public class RoleNotFoundException(string Role): 
        NotFoundException(message: $"Role {Role} not Found in Role")
    {
    }
}
