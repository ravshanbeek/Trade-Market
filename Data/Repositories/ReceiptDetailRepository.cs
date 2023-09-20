using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private TradeMarketDbContext context;

        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }

        public Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return Task.FromResult(context.ReceiptsDetails
                .AsEnumerable<ReceiptDetail>());
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await context.ReceiptsDetails.FindAsync(id);
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(ReceiptDetail entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await context.ReceiptsDetails.FindAsync(id);

            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public void Update(ReceiptDetail entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await Task.FromResult(context.ReceiptsDetails
                .Include(x=>x.Product)
                .Include(x=>x.Receipt)
                .Include(x=>x.Product.Category)
                .AsEnumerable<ReceiptDetail>());
        }
    }
}