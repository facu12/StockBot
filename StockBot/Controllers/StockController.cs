using Entities;
using Microsoft.AspNetCore.Mvc;
using StockBot.Services.Interfaces;
using System;

namespace StockBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private IStockService _stockService;
        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        [Route("GetStock")]
        public ActionResult<Stock> GetStock(string code)
        {
            try
            {
                var result = _stockService.GetStock(code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
