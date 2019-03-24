using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class WorkHoursForPlaceUpdateDTO
    {
        [MaxLength(50)]
        public string Id { get; set; }

        [MaxLength(36)]
        public string PlaceId { get; set; }

        [Required]
        [Range(1, 10)]
        public WeekDays Day { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan Open { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan Close { get; set; }
    }
}
