using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models
{
    public class PlaceFaq
    {
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(500,ErrorMessage ="Question cannot exceed 500 characters")]
        public string Question { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        public string Answer { get; set; }

        [MaxLength(36)]
        public string PlaceId { get; set; }

        public Place Place { get; set; }
    }
}
