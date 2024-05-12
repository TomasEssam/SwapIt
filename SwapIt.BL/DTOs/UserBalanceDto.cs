using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class UserBalanceDto
    {
        public int UserBalanceId { get; set; }
        public float Amount { get; set; }
        public int Points { get; set; }
        public int UserId { get; set; }
    }
}
