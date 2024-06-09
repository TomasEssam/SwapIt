using SwapIt.Data.Constants;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SwapIt.BL.DTOs
{
    public class ServiceRequestDto
    {

        public int Id { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public string RequestState { get; set; } 
        public float? ExecutionTime { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public IFormFile? RequestImage { get; set; }

    }
}
