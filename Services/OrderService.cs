using ProvaPub.Factories;
using ProvaPub.Models;

namespace ProvaPub.Services
{
	public class OrderService
	{
        private readonly PaymentStrategyFactory _paymentStrategyFactory;

        public OrderService(PaymentStrategyFactory paymentStrategyFactory)
        {
            _paymentStrategyFactory = paymentStrategyFactory;
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            var paymentStrategy = _paymentStrategyFactory.GetPaymentStrategy(paymentMethod);
            return await paymentStrategy.ProcessPayment(paymentValue, customerId);
        }
    }
}
