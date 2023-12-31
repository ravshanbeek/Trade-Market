﻿using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
 using System.Text.Json;
 using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [Route("api/{controller}")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private IReceiptService receiptService;
        public ReceiptsController(IReceiptService receiptService) =>
            this.receiptService = receiptService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get() =>
            Content(JsonSerializer.Serialize(
                await receiptService.GetAllAsync()));
        

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetById(int id) =>
            Content(JsonSerializer.Serialize(await receiptService.GetByIdAsync(id)));

        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByIdWithDetails(int id) =>
            Content(JsonSerializer.Serialize(await receiptService.GetReceiptDetailsAsync(id)));
        
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<ReceiptModel>> GetSum(int id) =>
            Content(JsonSerializer.Serialize(await receiptService.ToPayAsync(id)));

        [HttpGet("period")]
        public async Task<ActionResult<ReceiptModel>> GetReceiptsByPeriod([FromQuery] DateTime startDate, DateTime endDate) =>
        
            Content(JsonSerializer.Serialize(await receiptService.GetReceiptsByPeriodAsync(startDate, endDate)));

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ReceiptModel receipt)
        {
            receipt.Id = Id;
            await receiptService.UpdateAsync(receipt);

            return Ok();
        }

        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProduct(int id, int productId, int quantity, [FromBody] ReceiptDetailModel receiptDetail)
        {
            await receiptService.RemoveProductAsync(productId, id, quantity);

            return Ok();
        }

        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> CheckOut(int id)
        {
            await receiptService.CheckOutAsync(id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await receiptService.DeleteAsync(id);

            return Ok();
        }
    }
}