using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SpecificationsParam.Invoice
{
    public class InvoiceSpecificationsParamters
    {
        public int? ShopId { get; set; }

        public string? DisplayName { get; set; }
        
        
        public DateTime? CreateAt { get; set; }
        public string? sort { get; set; }
        public string? Search { get; set; }

        private int _pageIndex = 1;
        private int _pageSize = 10;

        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}
