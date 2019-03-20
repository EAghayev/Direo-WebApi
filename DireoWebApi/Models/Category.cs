using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models
{
    public class Category
    {
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(150,ErrorMessage ="Category name cannot exceed 150 characters")]
        public string Name { get; set; }

        public List<Place> Places { get; set; }
    }
}
