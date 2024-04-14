using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs.Identity
{
    public class ResetPasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
