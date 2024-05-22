using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class SearchResultDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public int ServicePrice { get; set; }
        public string CategoryName { get; set; }
        public float totalRate { get; set; }
        public string Username { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? Notes { get; set; }
        public int? ServiceRequestId { get; set; }
    }
}
