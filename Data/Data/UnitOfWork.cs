using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Data.Repositories;

namespace Data.Data
{
    // create class UnitOfWork
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext context;
        private ICustomerRepository customerRepository;
        private IPersonRepository personRepository;
        private IProductRepository productRepository;
        private IProductCategoryRepository productCategoryRepository;
        private IReceiptRepository receiptRepository;
        private IReceiptDetailRepository receiptDetailRepository;

        public UnitOfWork(TradeMarketDbContext context)
        {
            this.context = context;
        }

        public ICustomerRepository CustomerRepository
        {
            get => customerRepository ?? new CustomerRepository(context);
        }
        public IPersonRepository PersonRepository
        {
            get => personRepository ?? new PersonRepository(context);
        }
        public IProductRepository ProductRepository
        {
            get => productRepository ?? new ProductRepository(context);
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get => productCategoryRepository ?? new ProductCategoryRepository(context);
        }

        public IReceiptRepository ReceiptRepository
        {
            get => receiptRepository ?? new ReceiptRepository(context);
        }

        public IReceiptDetailRepository ReceiptDetailRepository
        {
            get => receiptDetailRepository ?? new ReceiptDetailRepository(context);
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
