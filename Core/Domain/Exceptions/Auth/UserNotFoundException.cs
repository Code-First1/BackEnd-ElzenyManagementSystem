using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Auth
{
    public class UserNotFoundException(string username) :
        NotFoundException(message: $"user {username} not Found in users")
    {
    }
}
