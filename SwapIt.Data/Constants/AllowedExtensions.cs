using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Constants
{
    public static class AllowedExtensions
    {
        public static string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        public static bool isValid(string extension)
        {
            var lEx = extension.ToLowerInvariant();
            return allowedExtensions.Contains(lEx);
        }
    }
}
