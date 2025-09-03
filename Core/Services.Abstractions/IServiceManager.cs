using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
        ICategoryService CategoryService { get; }
        IInvoiceService InvoiceService { get; }

        ISubCategoryService SubCategoryService { get; }
        IProductService ProductService { get; }

        IInventoryProductService InventoryProductService { get; }
        IInventoryDashboardService InventoryDashboardService { get; }
        IShopProductService ShopProductService { get; }

        ITransactionPerProductService TransactionPerProductService { get; }
        IInventoryToShopTransactionService InventoryToShopTransactionService { get; }
        IAuthService AuthService { get; }
    }
}
