using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class ServiceDto
    {
        public int ServiceId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public int TimeToExecute { get; set; }
        public string PreviousworkImagesUrl { get; set; }
        public int ServiceProviderId { get; set; }
        public int CategoryId { get; set; }
        //Ask about it 
        public ICollection<RateDto>? Rates { get; set; }
        public ICollection<ServiceRequestDto>? ServiceRequests { get; set; }
    }
}
