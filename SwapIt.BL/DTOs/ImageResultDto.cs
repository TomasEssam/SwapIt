using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class ImageResultDto
    {
        public string? Base64Image { get; set; }
        public Stream ImageStream { get; set; }
        public string ContentType { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
