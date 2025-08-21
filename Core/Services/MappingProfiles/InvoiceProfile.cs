using AutoMapper;
using Domain.Models;
using Shared.DTOs.Invoice;

namespace Services.MappingProfiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            // Mapping Invoice → InvoiceResultDto
            CreateMap<Invoice, InvoiceResultDto>()
                .ForMember(dest => dest.Products,
                           opt => opt.MapFrom(src => src.InvoiceProducts));

            // Mapping InvoiceProduct → InvoiceProductResultDto
            CreateMap<InvoiceProduct, InvoiceProductResultDto>();
        }
    }
}

