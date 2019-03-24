using AutoMapper;
using DireoWebApi.Models;
using DireoWebApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DireoWebApi.Mappings
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserSignInDTO>().ReverseMap();
            CreateMap<UserSignInDTO, User>().ReverseMap();

            CreateMap<User, UserSignUpDTO>().ReverseMap();
            CreateMap<UserSignUpDTO, User>().ReverseMap();

            CreateMap<User, UserGetDTO>().ReverseMap();

            CreateMap<PlacePostDTO, Place>().ReverseMap();

            /*Faq Mapping*/
            CreateMap<PlaceFaq, FaqDTO>().ReverseMap();
            CreateMap<FaqDTO, PlaceFaq>().ReverseMap();

            /*Tag Mapping*/
            CreateMap<Tag, TagDTO>().ReverseMap();
            CreateMap<TagDTO, Tag>().ReverseMap();

            /*Hours Mapping*/
            CreateMap<WorkHour, WorkHoursDTO>().ReverseMap();
            CreateMap<WorkHoursDTO, WorkHour>().ReverseMap();

            /*Slider Photos Mapping*/
            CreateMap<PlaceSliderPhotos, PlaceSliderPhotoDTO>().ReverseMap();
            CreateMap<PlaceSliderPhotoDTO, PlaceSliderPhotos>().ReverseMap();

            /*Socials Mapping*/
            CreateMap<Social, SocialForUserOrPlaceDTO>().ReverseMap();
            CreateMap<SocialForUserOrPlaceDTO, Social>().ReverseMap();

            /*Place mapping*/
            CreateMap<Place, PlaceGetDTO>()
                .ForMember(dest=>dest.Tags,from=>from
                .MapFrom(s=>s.PlacesTags.Select(pt=>pt.Tag)));
        }
    }
}
