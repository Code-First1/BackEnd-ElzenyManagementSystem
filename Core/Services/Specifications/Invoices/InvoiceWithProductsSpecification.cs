using Domain.Models;
using Shared.SpecificationsParam.Invoice;

namespace Services.Specifications.Invoices
{
    public class InvoiceWithProductsSpecification : BaseSpecifications<Invoice, int>
    {
       
        public InvoiceWithProductsSpecification(int id)
            : base(i => i.Id == id)
        {
            ApplyInclude();
        }

      
        public InvoiceWithProductsSpecification(InvoiceSpecificationsParamters invoiceParams)
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
            ApplyInclude();

            ApplySorting(invoiceParams.sort);

            ApplyPagination(invoiceParams.PageIndex, invoiceParams.PageSize);
        }

        private void ApplyInclude()
        {
            AddInclude(i => i.User);
            AddInclude(i => i.Shop);
            AddInclude(i => i.InvoiceProducts);
           
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "datedesc":
                        AddOrderByDescending(i => i.CreateAt);
                        break;
                    case "dateasc":
                        AddOrderBy(i => i.CreateAt);
                        break;
                    case "totaldesc":
                        AddOrderByDescending(i => i.TotalPrice);
                        break;
                    case "totalasc":
                        AddOrderBy(i => i.TotalPrice);
                        break;
                    default:
                        AddOrderByDescending(i => i.CreateAt);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(i => i.CreateAt);
            }
        }
    }
}
