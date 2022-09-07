using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.CustomServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesParserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        [HttpPost("addIfNotExists")]
        public async Task<IActionResult> AddIfNotExists(PlatformDTO platformDTO)
        {
            await _platformService.AddIfNotExists(platformDTO);
            return Ok();
        }

        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }
    }
}
