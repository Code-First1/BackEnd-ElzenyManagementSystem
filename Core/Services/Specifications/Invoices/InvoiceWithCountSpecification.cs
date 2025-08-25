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
                    (!string.IsNullOrEmpty(i.UserId) || i.UserId == invoiceParams.UserId) &&
                    (!invoiceParams.CreateAt.HasValue || i.CreateAt.Date == invoiceParams.CreateAt.Value.Date) &&
                    (string.IsNullOrEmpty(invoiceParams.Search) ||
                        i.User.UserName.ToLower().Contains(invoiceParams.Search.ToLower()) ||
                        i.Shop.Name.ToLower().Contains(invoiceParams.Search.ToLower()))
            )
        {
        }
    }
}
