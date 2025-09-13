using BackEnd_ElzenyManagementSystem.Extensions;
using Domain.Contracts;
using System.Threading.Tasks;

namespace BackEnd_ElzenyManagementSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _currentNumber = 0;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

  
        public int GetNextInvoiceNumber()
        {
            _currentNumber++;
            return _currentNumber;
        }

       
        public Task ResetInvoiceNumberAsync()
        {
            _currentNumber = 0;
            return Task.CompletedTask;
        }
    }
}
