using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService service;

        public ProductsController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get()
        {
            var products = JsonSerializer.Serialize(await service.GetAllAsync());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product = JsonSerializer.Serialize(await service.GetByIdAsync(id));
            return Ok(product);
        }
        
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter(FilterSearchModel model)
        {
            var products = JsonSerializer.Serialize(await service.GetByFilterAsync(model));
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel model)
        {
            await service.AddAsync(model);
            return Ok(model);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetAllCategories()
        {
            var categories = JsonSerializer.Serialize(await service.GetAllProductCategoriesAsync());
            return Ok(categories);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> Update(int id, [FromBody] ProductModel model)
        {
            model.Id = id;
            await service.UpdateAsync(model);
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await service.DeleteAsync(id);
            return Ok(id);
        }
        
        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel model)
        {
            await service.AddCategoryAsync(model);
            return Ok();
        }

        [HttpPut("categories")]
        public async Task<ActionResult<int>> UpdateCategory(int id, ProductCategoryModel model)
        {
            model.Id = id;
            await service.UpdateCategoryAsync(model);
            return Ok(id);
        }

        [HttpDelete("categories{id}")]
        public async Task<ActionResult<int>> DeleteCategoty(int id)
        {
            await service.RemoveCategoryAsync(id);
            return Ok(id);
        }
    }
}