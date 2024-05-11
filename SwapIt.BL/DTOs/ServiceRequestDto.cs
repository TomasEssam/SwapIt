using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class ServiceRequestDto
    {
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public string RequestState { get; set; }
        public float? ExecutionTime { get; set; }
        public float PaidFund { get; set; } = 0;
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
    }
}
