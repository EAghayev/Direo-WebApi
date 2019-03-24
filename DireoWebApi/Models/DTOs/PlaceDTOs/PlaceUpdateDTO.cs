using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Models.DTOs
{
    public class PlaceUpdateDTO
    {
        [MaxLength(36)]
        public string Id { get; set; }

        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }

        public decimal Ranking { get; set; }

        public int ReviewCount { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Desc { get; set; }

        [MaxLength(250, ErrorMessage = "Short Desc cannot exceed 250 characters")]
        public string ShortDesc { get; set; }

        public string Photo { get; set; }

        [MaxLength(100, ErrorMessage = "Photo name cannot exceed 100 characters")]
        public string PhotoFileName { get; set; }

        [MaxLength(250)]
        public string Video { get; set; }

        [Column(TypeName = "tinyint")]
        public Gender Gender { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [MaxLength(100, ErrorMessage = "Phone cannot exceed 100 characters")]
        public string Phone { get; set; }

        public DateTime? CreateAt { get; set; }

        public bool HideContactInfo { get; set; }

        public bool HideMap { get; set; }

        [MaxLength(100)]
        public string Lat { get; set; }

        [MaxLength(100)]
        public string Long { get; set; }

        [MaxLength(150, ErrorMessage = "Website cannot exceed 150 characters")]
        public string Website { get; set; }

        [MaxLength(150, ErrorMessage = "Address cannot exceed 150 characters")]
        public string Address { get; set; }

        [MaxLength(36)]
        public string CityId { get; set; }
        [MaxLength(36)]
        public string CategoryId { get; set; }
        [MaxLength(36)]
        public string UserId { get; set; }
    }
}
