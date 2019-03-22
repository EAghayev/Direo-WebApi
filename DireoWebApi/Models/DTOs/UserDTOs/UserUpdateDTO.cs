using DireoWebApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class UserUpdateDTO
    {
        [MaxLength(36)]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(120, ErrorMessage = "FullName cannot exceed 120 characters")]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string NewPassword { get; set; }

        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Required]
        public string Email { get; set; }

        [StringLength(250, ErrorMessage = "Website cannot exceed 250 characters")]
        public string Website { get; set; }

        [MaxLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        public string Phone { get; set; }

        [MaxLength(150, ErrorMessage = "Address cannot exceed 150 characters")]
        public string Address { get; set; }

        [Column(TypeName = "tinyint")]
        public Gender? Gender { get; set; }

        public List<SocialForUserOrPlaceDTO> Socials { get; set; }
    }
}
