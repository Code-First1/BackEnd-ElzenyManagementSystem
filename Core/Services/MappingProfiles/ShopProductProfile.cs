using AutoMapper;
using Domain.Models;
using Shared.DTOs.InventoryProduct;
using Shared.DTOs.ShopProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    internal class ShopProductProfile : Profile
    {
        public ShopProductProfile()
        {
            CreateMap<ShopProduct, ShopProductResultDto>().ReverseMap();
            CreateMap<ShopProduct, ShopProductUpdateDto>().ReverseMap();

        }
    }
}
