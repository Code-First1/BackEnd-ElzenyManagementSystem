using AutoMapper;
using Domain.Models;
using Shared.DTOs.Invoice;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceResultDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.CreateAt)) 
            .ForMember(dest => dest.InvoiceProduct, opt => opt.MapFrom(src => src.InvoiceProducts));

        CreateMap<InvoiceProduct, InvoiceProductResultDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
