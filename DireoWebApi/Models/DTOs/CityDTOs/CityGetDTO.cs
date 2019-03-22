using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class CityGetDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryId { get; set; }

        public CountryGetDTO Country { get; set; }
    }
}
