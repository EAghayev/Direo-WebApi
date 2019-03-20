using AutoMapper;
using DireoWebApi.Models;
using DireoWebApi.Models.NewFolder.DTOs.UserDTOs;
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

        }
    }
}
