using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private TradeMarketDbContext context;

        public PersonRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task<IEnumerable<Person>> GetAllAsync()
        {
            return Task.FromResult(context.Persons.AsEnumerable<Person>());
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await context.Persons.FindAsync(id);
        }

        public async Task AddAsync(Person entity)
        {
            await context.Persons.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public  async void Delete(Person entity)
        {
            context.Persons.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = context.Persons.Find(id);
            context.Persons.Remove(entity);
            await context.SaveChangesAsync();
        }

        public void Update(Person entity)
        {
            context.Update(entity);
        }
    }
}