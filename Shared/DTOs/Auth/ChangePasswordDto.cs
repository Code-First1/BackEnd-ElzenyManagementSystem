using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public string UserName { get; set; }
        public string CurrrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
