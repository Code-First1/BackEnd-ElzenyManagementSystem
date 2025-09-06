using Domain.Models;
using Shared.SpecificationsParam.Invoice;

namespace Services.Specifications.Invoices
{
    public class InvoiceWithCountSpecification : BaseSpecifications<Invoice, int>
    {
        public InvoiceWithCountSpecification(InvoiceSpecificationsParamters invoiceParams)
            : base(
                i =>
                    (!invoiceParams.ShopId.HasValue || i.ShopId == invoiceParams.ShopId) &&
                    (!invoiceParams.CreateAt.HasValue || i.CreateAt.Date == invoiceParams.CreateAt.Value.Date) &&
                    (string.IsNullOrEmpty(invoiceParams.DisplayName) || i.UserName.ToLower() == invoiceParams.DisplayName.ToLower()) &&
                    (string.IsNullOrEmpty(invoiceParams.Search) ||
                        i.Shop.Name.ToLower().Contains(invoiceParams.Search.ToLower()))
            )
        {
        }
    }
}
