using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class RateDto
    {
        public int? Id { get; set; }
        [Required]
        public int RateValue { get; set; }
        [Required]
        public DateTime RateDate { get; set; } = DateTime.Now;
        [MaxLength(1000)]
        public string? Feedback { get; set; }
        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }
}
