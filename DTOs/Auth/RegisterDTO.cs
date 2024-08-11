using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Auth
{
    public class RegisterDTO
    {
        public string ArabicUserName { get; set; }
        [Required]
        public string EnglishUserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public string Lang { get; set; } = "en";

    }
}
