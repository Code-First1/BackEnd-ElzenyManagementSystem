using Shared.DTOs.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Response
{
    public class InvoicePaginationResponse<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<InvoiceResultDto> Data { get; set; }
        public decimal? GrandTotal { get; set; }
        public InvoicePaginationResponse(int pageIndex, int pageSize, int totalCount, IEnumerable<InvoiceResultDto> data, decimal grandTotal)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
            GrandTotal = grandTotal;
        }
    }

}
