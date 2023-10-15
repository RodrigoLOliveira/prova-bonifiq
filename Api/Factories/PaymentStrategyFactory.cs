using ProvaPub.Interfaces;
using ProvaPub.Strateties.Payments;

namespace ProvaPub.Factories
{
    public class PaymentStrategyFactory
    {
        public IPaymentStrategy GetPaymentStrategy(string paymentMethod)
        {
            //Melhoria: Poderia carregar automaticamente pelo assembly
            return paymentMethod switch
            {
                "Pix" => new PixPaymentStrategy(),
                "CreditCard" => new CreditCardPaymentStrategy(),
                "Paypal" => new PaypalPaymentStrategy(),
                _ => throw new NotSupportedException($"Payment method {paymentMethod} not supported")
            };
        }
    }
}
