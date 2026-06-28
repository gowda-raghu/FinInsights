using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using System.Xml.Serialization;

namespace Test.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Stocks")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;


        private readonly IStockService _stockService;
        public StocksController(ILogger<LoginController> logger, IStockService stockservice)
        {
            _logger = logger;
            _stockService = stockservice;

        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchStocks(string searchinput)
        {
            var result = await _stockService.SearchStocks(searchinput);
            return Ok(result);
        }


        [HttpGet("StockDetails")]
        public async Task<IActionResult> StockDetails(string symbol)
        {
            var result = await _stockService.GetStockDetails(symbol);
            return Ok(result);
        }


    }

}