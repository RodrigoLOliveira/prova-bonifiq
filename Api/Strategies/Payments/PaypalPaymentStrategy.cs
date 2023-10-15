using ProvaPub.Interfaces;
using ProvaPub.Models;

namespace ProvaPub.Strateties.Payments
{
    public class PaypalPaymentStrategy : IPaymentStrategy
    {
        public async Task<Order> ProcessPayment(decimal paymentValue, int customerId)
        {
            return new Order()
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Value = paymentValue
            };
        }
    }
}
