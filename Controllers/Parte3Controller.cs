using Microsoft.AspNetCore.Mvc;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;

namespace ProvaPub.Controllers
{

    /// <summary>
    /// Esse teste simula um pagamento de uma compra.
    /// O método PayOrder aceita diversas formas de pagamento. Dentro desse método é feita uma estrutura de diversos "if" para cada um deles.
    /// Sabemos, no entanto, que esse formato não é adequado, em especial para futuras inclusões de formas de pagamento.
    /// Como você reestruturaria o método PayOrder para que ele ficasse mais aderente com as boas práticas de arquitetura de sistemas?
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Parte3Controller : ControllerBase
    {
        readonly OrderService _orderService;
        readonly TestDbContext _ctx;

        public Parte3Controller(OrderService orderService, TestDbContext ctx)
        {
            _orderService = orderService;
            _ctx = ctx;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<Order>> PlaceOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            try
            {
                var order = await _orderService.PayOrder(paymentMethod, paymentValue, customerId);
                _ctx.Add(order);
                await _ctx.SaveChangesAsync();
                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException?.Message ?? e.Message);
            }
        }
    }

}
