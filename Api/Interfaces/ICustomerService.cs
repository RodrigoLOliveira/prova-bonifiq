using ProvaPub.Models;

namespace ProvaPub.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedList<Customer>> ListAsync(int page);

        Task<bool> CanPurchase(int customerId, decimal purchaseValue);
    }
}
