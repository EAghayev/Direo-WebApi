using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class PlaceGetDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Ranking { get; set; }

        public int ReviewCount { get; set; }

        public string Desc { get; set; }

        public string ShortDesc { get; set; }

        public string Photo { get; set; }

        public string PhotoFileName { get; set; }

        public string Video { get; set; }

        public Gender Gender { get; set; }

        public decimal Price { get; set; }

        public string Phone { get; set; }

        public DateTime? CreateAt { get; set; }

        public bool HideContactInfo { get; set; }

        public bool HideMap { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public string Website { get; set; }

        public string Address { get; set; }

        public string CityId { get; set; }

        public string CategoryId { get; set; }

        public string UserId { get; set; }

        public CategoryGetDTO Category { get; set; }
        public UserGetForPlaceDTO User { get; set; }
        public CityGetDTO City { get; set; }
        public List<TagGetDTO> Tags { get; set; }
        public List<FaqGetDTO> PlaceFaqs { get; set; }
        public List<WorkHourGetDTO> WorkHours { get; set; }
        public List<SliderPhotoGetDTO> PlaceSliderPhotos { get; set; }
        public List<SocialGetDTO> Socials { get; set; }
    }
}
