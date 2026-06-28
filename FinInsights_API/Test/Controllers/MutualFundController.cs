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
    [Route("api/v{version:apiVersion}/Mf")]
    public class MFController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;


        private readonly IMfService _mfService;
        public MFController(ILogger<LoginController> logger, IMfService mfService)
        {
            _logger = logger;
            _mfService = mfService;

        }

        [HttpGet("GetSchemes")]
        public async Task<IActionResult> GetSchemes(int limit, int page)
        {
            List<Scheme> schemes = await _mfService.GetSchemes(limit, page);
            return Ok(schemes);
        }

        [HttpGet("GetNavData")]
        public async Task<IActionResult> getNavData(string schemeCode, string startDate, string endDate)
        {
            Console.WriteLine(schemeCode);
            int code = Int32.Parse(schemeCode);
            FundResponse response = await _mfService.GetNavData(code, startDate, endDate);
            return Ok(response);
        }

        [HttpGet("GetLatestNavData")]
        public async Task<IActionResult> GetLatestNavData(string schemeCode)
        {
            Console.WriteLine(schemeCode);
            int code = Int32.Parse(schemeCode);
            FundResponse response = await _mfService.GetLatestNavData(code);
            return Ok(response);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchinput)
        {
            Console.WriteLine(searchinput);
            List<Scheme> response = await _mfService.SearchSchema(searchinput);
            return Ok(response);
        }

        [HttpPut("AddFav")]
        public async Task<IActionResult> AddFav(FavouriteDto favouriteDto)
        {
            int response = await _mfService.AddFav(favouriteDto);
            return Ok(response);
        }

        [HttpPut("RemoveFav")]
        public async Task<IActionResult> RemoveFav(FavouriteDto favouriteDto)
        {
            int response = await _mfService.RemoveFav(favouriteDto);
            return Ok(response);
        }

        [HttpGet("GetAllFav")]
        public async Task<IActionResult> GetAllFav(Guid userId)
        {
            List<Favourite> response = await _mfService.GetAllFav(userId);
            return Ok(response);
        }

        [HttpPost("MailFav")]
        public async Task<IActionResult> MailFav(MailFavDto mailFavDto)
        {
            var response = await _mfService.MailFav(mailFavDto);
            return Ok(response);
        }

    }

}