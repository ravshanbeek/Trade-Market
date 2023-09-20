using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private TradeMarketDbContext context;

        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return Task.FromResult(context.ProductCategories.AsEnumerable<ProductCategory>());
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await context.ProductCategories.FindAsync(id);
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await context.ProductCategories.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(ProductCategory entity)
        {
            context.ProductCategories.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entiry = await context.ProductCategories.FindAsync(id);
            context.Remove(entiry);
            await context.SaveChangesAsync();
        }

        public void Update(ProductCategory entity)
        {
            context.ProductCategories.Update(entity);
        }
    }
}