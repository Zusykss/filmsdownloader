using Core.Classes;
using Core.DTOs;
using Core.Interfaces.CustomServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MoviesParserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialController : ControllerBase
    {
        private readonly ISerialService _serialService;

        public SerialController(ISerialService serialService)
        {
            _serialService = serialService;
        }

        [HttpGet("getSerials")]
        public IActionResult GetSerials([FromQuery] QueryStringParameters queryStringParameters)
        {
            var movies = _serialService.GetByPage(queryStringParameters);
            var metadata = new
            {
                movies.TotalCount,
                movies.PageSize,
                movies.CurrentPage,
                movies.TotalPages,
                movies.HasNext,
                movies.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(movies);
        }

        [HttpPost("addSerial")]
        public async Task<IActionResult> AddSerial(SerialDTO serialDTO)
        {
            await _serialService.Add(serialDTO);
            return Ok();
        }
    }
}
