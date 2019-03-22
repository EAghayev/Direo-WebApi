using DireoWebApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class UserGetDTO
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Profile { get; set; }

        public string ProfileFileName { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public Gender? Gender { get; set; }

        public List<SocialForUserOrPlaceDTO> Socials { get; set; }
    }
}
