using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private TradeMarketDbContext context;

        public ReceiptRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Receipt>> GetAllAsync() =>
            await Task.FromResult(context.Receipts.AsEnumerable<Receipt>());
        

        public async Task<Receipt> GetByIdAsync(int id) =>
            await context.Receipts.FindAsync(id);

        public async Task AddAsync(Receipt entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(Receipt entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await context.Receipts.FindAsync(id);

            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public void Update(Receipt entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await Task.FromResult(context.Receipts
                .Include(x => x.Customer)
                .Include(x => x.ReceiptDetails)
                .ThenInclude(x =>x.Product)
                .ThenInclude(x => x.Category)
                .AsEnumerable<Receipt>());
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await context.Receipts
                .Include(x => x.Customer)
                .Include(x => x.ReceiptDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}