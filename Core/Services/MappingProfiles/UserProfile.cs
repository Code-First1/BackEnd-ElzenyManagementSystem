using AutoMapper;
using Domain.Models;
using Domain.Models.Identity;
using Shared.DTOs.Product;
using Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<AppUser, UserProfileDto>()
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        }
    }
}
