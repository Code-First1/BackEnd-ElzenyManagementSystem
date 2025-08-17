using AutoMapper;
using Domain.Models;
using Shared.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryResultDto>().ReverseMap();
            CreateMap<Category,CategoryCreateDto>();
            CreateMap<Category,CategoryUpdateDto>();
        }
    }
}
