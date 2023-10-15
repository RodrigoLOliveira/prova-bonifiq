using ProvaPub.Models;

namespace ProvaPub.Interfaces
{
    public interface IPaymentStrategy
    {
        Task<Order> ProcessPayment(decimal paymentValue, int customerId);
    }
}
