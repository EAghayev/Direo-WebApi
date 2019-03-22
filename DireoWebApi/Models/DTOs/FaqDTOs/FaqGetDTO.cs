using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class FaqGetDTO
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
