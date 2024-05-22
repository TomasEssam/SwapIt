using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs.Identity
{
    public class UserDto
    {
        public int UserId { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string Username { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain at least 8 characters including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

      
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        //we can remove it if we want
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1900", "12/31/2008", ErrorMessage = "The person must be at least 16 years old.")]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression("^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [RegularExpression("^01[0-9]{9}$", ErrorMessage = "Phone number must be 11 digits and start with '01'.")]
        public string PhoneNumber { get; set; }
        [StringLength(100, MinimumLength = 2)]
        public string? Address { get; set; }
        public string? JobTitle { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? RoleId { get; set; }

      
    }
}
