using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications.Invoices;
using Shared.DTOs.Invoice;
using Shared.Response;
using Shared.SpecificationsParam.Invoice;

namespace Services
{
    public class InvoiceService(IUnitOfWork unitOfWork, IMapper mapper) : IInvoiceService
    {
      
        public async Task<int> AddInvoiceAsync(InvoiceCreateDto dto)
        {
           
            var invoice = mapper.Map<Invoice>(dto);

         
            invoice.TotalPrice = dto.Items.Sum(i => i.TotalPricePerItem);

            await unitOfWork.GetRepository<Invoice, int>().AddAsync(invoice);
           

            return invoice.Id;
        }

      
        public async Task<bool> UpdateInvoiceAsync(int id, InvoiceUpdateDto dto)
        {
            var repo = unitOfWork.GetRepository<Invoice, int>();

            var invoice = await repo.GetAsync(id);
            if (invoice == null)
                return false;

            mapper.Map(dto, invoice);

          

            repo.Update(invoice);
           
            return true;
        }

     
        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var repo = unitOfWork.GetRepository<Invoice, int>();

            var invoice = await repo.GetAsync(id);
            if (invoice == null)
                return false;

            repo.Delete(invoice);
           
            return true;
        }

     
        public async Task<InvoiceResultDto?> GetInvoiceByIdAsync(int id)
        {
            var spec = new InvoiceWithProductsSpecification(id);
            var invoice = await unitOfWork.GetRepository<Invoice, int>().GetAsync(spec);

            return mapper.Map<InvoiceResultDto>(invoice);
        }

     
        public async Task<PaginationResponse<InvoiceResultDto>> GetInvoicesAsync(InvoiceSpecificationsParamters invoiceSpecsParams)
        {
            var spec = new InvoiceWithProductsSpecification(invoiceSpecsParams);
            var invoices = await unitOfWork.GetRepository<Invoice, int>().GetAllAsync(spec);

            var specCount = new InvoiceWithCountSpecification(invoiceSpecsParams);
            var count = await unitOfWork.GetRepository<Invoice, int>().CountAsync(specCount);

            var result = mapper.Map<IEnumerable<InvoiceResultDto>>(invoices);

            return new PaginationResponse<InvoiceResultDto>(
                pageIndex: invoiceSpecsParams.PageIndex,
                pageSize: invoiceSpecsParams.PageSize,
                totalCount: count,
                data: result
            );
        }
    }
}
