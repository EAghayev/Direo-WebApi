using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DireoWebApi.Models
{
    public class Tag
    {
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage ="Tag name cannot exceed 100 characters")]
        public string Name { get; set; }

        public List<PlacesTags> PlacesTags { get; set; }
    }
}
