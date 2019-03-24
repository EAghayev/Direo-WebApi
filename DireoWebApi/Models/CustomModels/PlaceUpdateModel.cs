using DireoWebApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.CustomModels
{
    public class PlaceUpdateModel
    {
        public PlaceUpdateDTO Place { get; set; }
        public List<SocialForPlaceUpdateDTO> Socials { get; set; }
        public List<SliderForPlaceUpdateDTO> SliderPhotos { get; set; }
        public List<TagDTO> Tags { get; set; }

        public List<FaqForPlaceUpdateDTO> Faqs { get; set; }
        public List<WorkHoursForPlaceUpdateDTO> WorkHours { get; set; }
    }
}
