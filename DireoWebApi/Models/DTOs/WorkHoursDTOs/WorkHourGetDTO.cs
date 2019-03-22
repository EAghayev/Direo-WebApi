using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class WorkHourGetDTO
    {
        public string Id { get; set; }

        public WeekDays Day { get; set; }

        public TimeSpan Open { get; set; }

        public TimeSpan Close { get; set; }
    }
}
