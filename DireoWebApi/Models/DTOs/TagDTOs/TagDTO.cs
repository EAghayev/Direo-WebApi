using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class TagDTO
    {
        [MaxLength(36)]
        public string Id { get; set; }
    }
}
