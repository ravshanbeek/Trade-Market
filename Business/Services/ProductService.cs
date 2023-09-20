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
    public class ProductService : IProductService
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return mapper.Map<ProductModel>(product);
        }

        public async Task AddAsync(ProductModel model)
        {
            Validation(model);
            var mappedProduct = mapper.Map<Product>(model);
            await unitOfWork.ProductRepository.AddAsync(mappedProduct);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            Validation(model);
            var mappedProduct = mapper.Map<Product>(model);
            unitOfWork.ProductRepository.Update(mappedProduct);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWork.ProductRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            if (filterSearch.CategoryId != null)
                products = products.Where(p => p.ProductCategoryId == filterSearch.CategoryId);
            if (filterSearch.MinPrice != null)
                products = products.Where(p => p.Price > filterSearch.MinPrice);
            if (filterSearch.MaxPrice != null)
                products = products.Where(p => p.Price < filterSearch.MaxPrice);
            var mappedProducts = mapper.Map<IEnumerable<ProductModel>>(products);
            return mappedProducts;
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var categories = await unitOfWork.ProductCategoryRepository.GetAllAsync();
            var mappedCategories = mapper.Map<IEnumerable<ProductCategoryModel>>(categories);
            return mappedCategories;
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation(categoryModel);
            var mappedCategoryModel = mapper.Map<ProductCategory>(categoryModel);
            await unitOfWork.ProductCategoryRepository.AddAsync(mappedCategoryModel);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation(categoryModel);
            var mappedCategoryModel = mapper.Map<ProductCategory>(categoryModel);
            unitOfWork.ProductCategoryRepository.Update(mappedCategoryModel);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await unitOfWork.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await unitOfWork.SaveAsync();
        }

        private void Validation(ProductModel model)
        {
            if (model.Price < 0)
                throw new MarketException();
            if (model.ProductName is null || model.ProductName == "")
                throw new MarketException("ProductName is invalid");
        }

        private void Validation(ProductCategoryModel model)
        {
            if (model.CategoryName is null || model.CategoryName == "")
                throw new MarketException("CategoryName is invalid");
        }
    }
}