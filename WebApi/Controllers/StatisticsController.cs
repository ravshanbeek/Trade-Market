using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace WebApi.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IStatisticService statisticService;
        public StatisticsController(IStatisticService statisticService) =>
            this.statisticService = statisticService;
        

        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts([FromQuery] int productCount) =>
            Content(JsonSerializer.Serialize(await statisticService.GetMostPopularProductsAsync(productCount)));
        

        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts(int id, int productCount) =>
            Content(JsonSerializer.Serialize(await statisticService.GetCustomersMostPopularProductsAsync(productCount, id)));
        

        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetIncomeOfCategoryInPeriod(int categoryId, [FromQuery] DateTime startDate, DateTime endDate) =>
            Content(JsonSerializer.Serialize(await statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate)));
        

        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostValuableCustomers(int customerCount, [FromQuery] DateTime startDate, DateTime endDate) =>
             Content(JsonSerializer.Serialize(await statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate)));
        
    }
}