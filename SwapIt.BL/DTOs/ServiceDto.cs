using Microsoft.AspNetCore.Http;
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
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public int TimeToExecute { get; set; }
        public string? PreviousworkImagesUrl { get; set; }
        public int ServiceProviderId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? serviceImage { get; set; }

    }
}
