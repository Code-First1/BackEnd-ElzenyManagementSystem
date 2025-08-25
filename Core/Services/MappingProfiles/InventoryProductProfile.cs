using AutoMapper;
using Domain.Models;
using Shared.DTOs.InventoryProduct;
using Shared.DTOs.Transactions;

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
