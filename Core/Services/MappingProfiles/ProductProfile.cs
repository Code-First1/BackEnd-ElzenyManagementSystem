using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Shared.DTOs.Category;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDto>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(d => d.PictureUrl , o => o.MapFrom<PictureUrlResolver>());

            CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => Enum.Parse<Unit>(src.Unit, true)));

            CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => Enum.Parse<Unit>(src.Unit, true)));

        }
    }
}
