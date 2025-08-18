using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Shared.DTOs.Invoice;
using Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class InvoiceProfile: Profile
    {
        public InvoiceProfile() {

            // Map Invoice → InvoiceResultDto
            CreateMap<Invoice, InvoiceResultDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.Name : null));

            // Map InvoiceCreateDto → Invoice
            CreateMap<InvoiceCreateDto, Invoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore()); 

            // Map InvoiceUpdateDto → Invoice
            CreateMap<InvoiceUpdateDto, Invoice>()
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());


        }

    }
}
