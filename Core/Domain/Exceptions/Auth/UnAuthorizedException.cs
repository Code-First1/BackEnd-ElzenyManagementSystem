using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Auth
{
    public class UnAuthorizedException(string message= "Invalid Email Or Password") : Exception(message)
    {

    }
}
