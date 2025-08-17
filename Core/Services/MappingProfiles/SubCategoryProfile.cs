using AutoMapper;
using Domain.Models;
using Shared.DTOs.SubCategor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategory, SubCategoryResultDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryCreateDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryUpdateDto>().ReverseMap();

        }
    }
}
