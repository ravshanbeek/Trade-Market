using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private TradeMarketDbContext context;

        public CustomerRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task<IEnumerable<Customer>> GetAllAsync()
        {

            return Task.FromResult(context.Customers.AsEnumerable<Customer>());
        }

        public Task<Customer> GetByIdAsync(int id)
        {
            return Task.FromResult(context.Customers.Find(id));
        }

        public Task AddAsync(Customer entity)
        {
            context.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Customer entity)
        {
            context.Customers.Remove(entity);
        }

        public  async Task DeleteByIdAsync(int id)
        {
            var entity = await context.Customers.FindAsync(id);

            context.Customers.Remove(entity);
            context.SaveChanges();
        }

        public void Update(Customer entity)
        {
            context.Customers.Update(entity);
        }

        public Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return Task.FromResult(context.Customers
                .Include(x => x.Person)
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails).AsEnumerable());
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await context.Customers.AsNoTracking()
                .Include(x => x.Person)
                .Include (x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}