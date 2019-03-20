using System.ComponentModel.DataAnnotations;

namespace DireoWebApi.Models
{
    public class PlacesTags
    {
        [MaxLength(36)]
        public string Id { get; set; }
        [MaxLength(36)]
        public string PlaceId { get; set; }
        [MaxLength(36)]
        public string TagId { get; set; }

        public Place Place { get; set; }
        public Tag Tag { get; set; }

    }
}