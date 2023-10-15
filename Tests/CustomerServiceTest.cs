using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    /// <summary>
    /// Classe de teste para o serviço CustomerService.
    /// </summary>
    public class CustomerServiceTests
    {
        private readonly TestDbContext _context;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TestDbContext(options);

            // Verificar se já existem clientes ou produtos no banco de dados
            if (!_context.Customers.Any() && !_context.Products.Any())
            {
                _context.AddRange(_context.getCustomerSeed());
                _context.AddRange(_context.getProductSeed());
                _context.SaveChanges();
            }

            _customerService = new CustomerService(_context);
        }


        /// <summary>
        /// Testa se uma exceção ArgumentOutOfRangeException é lançada para entradas inválidas.
        /// </summary>
        [Theory]
        [InlineData(0, 100)]
        [InlineData(-1, 100)]
        [InlineData(1, 0)]
        [InlineData(1, -100)]
        public async Task CanPurchase_InvalidInput_ThrowsException(int customerId, decimal purchaseValue)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _customerService.CanPurchase(customerId, purchaseValue));
        }

        /// <summary>
        /// Testa se uma exceção InvalidOperationException é lançada para um cliente não existente.
        /// </summary>
        [Fact]
        public async Task CanPurchase_NonExistingCustomer_ThrowsException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _customerService.CanPurchase(999, 100));
        }

        /// <summary>
        /// Testa se o método retorna verdadeiro quando o cliente pode fazer uma compra.
        /// </summary>
        [Fact]
        public async Task CanPurchase_CustomerCanPurchase_ReturnsTrue()
        {
            var result = await _customerService.CanPurchase(2, 50);
            Assert.True(result);
        }

        /// <summary>
        /// Testa se o método retorna falso quando um cliente que nunca comprou antes excede o limite inicial de compra.
        /// </summary>
        [Fact]
        public async Task CanPurchase_FirstTimeCustomerExceedsInitialLimit_ReturnsFalse()
        {
            var result = await _customerService.CanPurchase(2, 150);
            Assert.False(result);
        }

        /// <summary>
        /// Testa se o método retorna verdadeiro quando um cliente com pedidos anteriores pode comprar acima do limite inicial.
        /// </summary>
        [Fact]
        public async Task CanPurchase_CustomerWithPreviousOrdersCanPurchaseAboveLimit_ReturnsTrue()
        {
            _context.Orders.Add(new Order
            {
                CustomerId = 1,
                Value = 10,
                OrderDate = DateTime.UtcNow.AddMonths(-2)
            });
            await _context.SaveChangesAsync();

            var result = await _customerService.CanPurchase(1, 150);

            Assert.True(result);
        }

        /// <summary>
        /// Testa se o método retorna falso quando uma compra já foi feita neste mês.
        /// </summary>
        [Fact]
        public async Task CanPurchase_CustomerAlreadyPurchasedThisMonth_ReturnsFalse()
        {
            // Adicionando uma ordem para o cliente 1 neste mês
            _context.Orders.Add(new Order
            {
                CustomerId = 3,
                Value = 10,
                OrderDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            var result = await _customerService.CanPurchase(3, 50);

            Assert.False(result);
        }

    }
}
