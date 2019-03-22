using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class UserSignUpDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Password { get; set; }

        [Required]
        [MaxLength(120, ErrorMessage = "FullName cannot exceed 120 characters")]
        public string FullName { get; set; }

        [Column(TypeName = "tinyint")]
        public Gender Gender { get; set; }
    }
}
