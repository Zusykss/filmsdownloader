using Core.DTOs.Parser;
using Core.Interfaces.CustomServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesParserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParserController : ControllerBase
    {
        private readonly IParserService _parserService;

        [HttpPost("startParser")]
        public IActionResult StartParser(ParserStartConfiguration parserStartConfiguration)
        {
            _parserService.StartParser(parserStartConfiguration);
            return Ok();
        }

        [HttpGet("stopParser")]
        public IActionResult StopParser()
        {
            _parserService.StopParser();
            return Ok();
        }

        [HttpGet("getParserState")]
        public IActionResult GetParserState()
        {
            return Ok(_parserService.GetParserState());
        }

        public ParserController(IParserService parserService)
        {
            _parserService = parserService;
        }
    }
}
