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

        [HttpGet("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _platformService.GetPlatformById(id));
        }

        [HttpGet("getAllPlatforms")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _platformService.GetAllPlatforms());
        }
        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }
    }
}
