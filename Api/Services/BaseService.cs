using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected readonly TestDbContext _ctx;

        protected BaseService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedList<T>> ListAsync(int page)
        {
            var itensPorPagina = 10;

            var totalCount = await _ctx.Set<T>().CountAsync();
            var customers = await _ctx.Set<T>()
                .Skip((page - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();

            var hasNext = ((page - 1) * itensPorPagina + customers.Count) < totalCount;

            return new PagedList<T>
            {
                Items = customers,
                TotalCount = totalCount,
                HasNext = hasNext
            };
        }

    }
}
