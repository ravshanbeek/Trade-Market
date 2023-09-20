using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private TradeMarketDbContext context;

        public ProductRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var k = this.context.Products;

            return await Task.FromResult(k.AsQueryable<Product>());
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product entity)
        {
            await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Product entity)
        {
            context.Products.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await context.Products.FindAsync(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public void Update(Product entity)
        {
            context.Products.Update(entity);
            context.SaveChanges();
        }

        public Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return Task.FromResult(context.Products
                .Include(x => x.Category)
                .Include(x =>x.ReceiptDetails)
                .AsEnumerable());
        }

        public Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return Task.FromResult(context.Products
                .Include(x => x.Category)
                .Include(x => x.ReceiptDetails)
                .AsEnumerable().FirstOrDefault(x => x.Id == id));
        }
    }
}