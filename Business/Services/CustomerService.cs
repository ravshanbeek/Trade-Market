using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = await unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var mappedCustomers = mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customers);
            return mappedCustomers;
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            var mappedCustomer = mapper.Map<Customer, CustomerModel>(customer);
            return mappedCustomer;
        }

        public async Task AddAsync(CustomerModel model)
        {
            Validation(model);
            var mappedCustomer = mapper.Map<CustomerModel, Customer >(model);
            await unitOfWork.CustomerRepository.AddAsync(mappedCustomer);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            Validation(model);
            var mappedModel = mapper.Map<CustomerModel, Customer>(model);
            unitOfWork.CustomerRepository.Update(mappedModel);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customer = (await unitOfWork.CustomerRepository.GetAllWithDetailsAsync())
                .Where(c => c.Receipts
                    .Any(r => r.ReceiptDetails
                        .Any(r => r.ProductId == productId)));
            return mapper.Map<IEnumerable<Customer>,IEnumerable<CustomerModel>>(customer);
        }

        private void Validation(CustomerModel model)
        {
            if (model is null)
                throw new MarketException();
            if (model.Name is null || model.Name == "")
                throw new MarketException("Name is empty");
            if (model.Surname is null || model.Surname == "")
                throw new MarketException("Surname is empty");
            if (model.BirthDate > DateTime.Now || model.BirthDate < new DateTime(1900, 1, 1))
                throw new MarketException("BirthDate is invalid");
            
        }
    }
}