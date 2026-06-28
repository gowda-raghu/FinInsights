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
    [Route("api/v{version:apiVersion}/News")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;


        private readonly INewsService _newsService;
        public NewsController(ILogger<LoginController> logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;

        }

        [HttpGet("GetLatestNews")]
        public async Task<IActionResult> GetLatestNews(string countryCode)
        {
            var newsResponseDto= await _newsService.GetNewsAsync(countryCode);
            return Ok(newsResponseDto);
        }
        
        [HttpGet("GetCountryList")]
        public async Task<IActionResult> GetCountryList()
        {
            var response= await _newsService.GetCountriesList();
            return Ok(response);
        }

        

    }

}