using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models
{
    public class PlaceSliderPhotos
    {
        [MaxLength(36)]
        public string Id { get; set; }
        [MaxLength(100)]
        public string Photo { get; set; }
        [MaxLength(100, ErrorMessage = "Photo name cannot exceed 10 characters")]
        public string PhotoName { get; set; }
        [MaxLength(36)]
        public string PlaceId { get; set; }

        public Place Place { get; set; }
    }
}
