using AutoMapper;
using Domain.Models;
using Shared.DTOs.InventoryProduct;

namespace Services.MappingProfiles
{
    public class InventoryProductProfile : Profile
    {
        public InventoryProductProfile()
        {
            CreateMap<InventoryProduct, InventoryProductResultDto>().ReverseMap();
            CreateMap<InventoryProduct, InventoryProductUpdateDto>().ReverseMap();

        }
    }
}
