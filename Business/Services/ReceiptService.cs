using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using AutoMapper;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receipts = await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            
            return mapper.Map<IEnumerable<ReceiptModel>>(receipts);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            
            return mapper.Map<ReceiptModel>(receipt);
        }

        public async Task AddAsync(ReceiptModel model)
        {
            Validation(model);
            var receipt = mapper.Map<Receipt>(model);
            await unitOfWork.ReceiptRepository.AddAsync(receipt);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            Validation(model);
            var receipt = mapper.Map<Receipt>(model);
            unitOfWork.ReceiptRepository.Update(receipt);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);

            foreach (var receiptDetail in receipt.ReceiptDetails)
            {
                unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
            }

            await unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            decimal productPrice = 0;

            if (unitOfWork.ProductRepository != null)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                    throw new MarketException("Product with this productId doesn`t exists");

                productPrice = product.Price;
            }

            if (unitOfWork.ReceiptRepository != null)
            {
                var receipt = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

                if (receipt != null && receipt.ReceiptDetails != null)
                {
                    var receiptDetails = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);

                    if (receiptDetails != null)
                    {
                        receiptDetails.Quantity += quantity;
                        await unitOfWork.SaveAsync();
                        return;
                    }
                }
            }

            decimal discount = (await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).Customer.DiscountValue;

            ReceiptDetail receiptDetail = new ReceiptDetail()
            {
                ProductId = productId,
                Quantity = quantity,
                ReceiptId = receiptId,
                DiscountUnitPrice = productPrice * ((100 - discount) / 100),
                UnitPrice = productPrice
            };

            await unitOfWork.ReceiptDetailRepository.AddAsync(receiptDetail);

            await unitOfWork.SaveAsync();

        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receiptDetails = (await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId))
                .ReceiptDetails.Where(r => r.ProductId == productId);
            foreach (var receiptDetail in receiptDetails)
            {
                if (receiptDetail.Quantity <= quantity)
                    unitOfWork.ReceiptDetailRepository.Delete(receiptDetail);
                else
                    receiptDetail.Quantity = quantity;
            }
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receiptDetail = (await unitOfWork
                    .ReceiptRepository
                    .GetByIdWithDetailsAsync(receiptId))
                .ReceiptDetails;
            return mapper.Map< IEnumerable <ReceiptDetail>, IEnumerable <ReceiptDetailModel> >(receiptDetail);
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipts = await unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            decimal result =  receipts.ReceiptDetails.Sum(x => x.Quantity * x.DiscountUnitPrice);

            return result;
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = (await unitOfWork.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(r => r.OperationDate > startDate && r.OperationDate < endDate);

            return mapper.Map<IEnumerable<ReceiptModel>>(receipts);
        }

        private void Validation(ReceiptModel model)
        {
            if (model is null)
                throw new MarketException("Model is null");
        }

        private void Validation(ProductModel model)
        {
            if (model is null)
                throw new MarketException("Model is null");
        }
    }
}