using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models
{
    public enum WeekDays
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7,
        InWeeks = 8,
        Weekend = 9,
        AllDays = 10
    }

    public class WorkHour
    {
        [MaxLength(50)]
        public string Id { get; set; }

        [Required]
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

        public Place Place { get; set; }
    }
}
