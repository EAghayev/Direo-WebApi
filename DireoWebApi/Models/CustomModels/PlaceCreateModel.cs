using DireoWebApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.CustomModels
{
    public class PlaceCreateModel
    {
        public PlacePostDTO Place { get; set; }
        public List<SocialForUserOrPlaceDTO> Socials { get; set; }
        public List<PlaceSliderPhotoDTO> SliderPhotos { get; set; }
        public List<TagDTO> Tags { get; set; }
        public List<FaqDTO> Faqs { get; set; }
        public List<WorkHoursDTO> WorkHours { get; set; }
    }
}
