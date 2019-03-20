using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.NewFolder.DTOs.SocialDTOs
{
    public class SocialForUserUpdateDTO
    {
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Social media name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = "Social media URL cannot exceed 250 characters")]
        public string Link { get; set; }
    }
}
