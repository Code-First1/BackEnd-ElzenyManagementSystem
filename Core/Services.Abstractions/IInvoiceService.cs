using Shared.DTOs.Invoice;
using Shared.DTOs.Product;
using Shared.Response;
using Shared.SpecificationsParam.Invoice;
using Shared.SpecificationsParam.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IInvoiceService
    {

        //GetAllProduct
        Task<PaginationResponse<InvoiceResultDto>> GetInvoicesAsync(InvoiceSpecificationsParamters invoiceSpecsParams);
        //GetById
        Task<InvoiceResultDto?> GetInvoiceByIdAsync(int id);

        Task<int> AddInvoiceAsync(InvoiceCreateDto dto);

        Task<InvoiceResultDto?> UpdateInvoiceAsync(int id, InvoiceUpdateDto dto);

        Task<bool> DeleteInvoiceAsync(int id);
    }
}
