using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class UserSignInDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Password { get; set; }
    }
}
