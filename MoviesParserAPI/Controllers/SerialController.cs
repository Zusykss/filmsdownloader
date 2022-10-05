using Core.Classes;
using Core.DTOs;
using Core.DTOs.Edit;
using Core.Exceptions;
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
        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id, [FromQuery] int statusId)
        {
            await _serialService.UpdateStatus(id, statusId);
            return Ok();
        }
        [HttpPost("updateNotes")]
        public async Task<IActionResult> UpdateNotes([FromQuery] int id, [FromBody] string notes)
        {
            await _serialService.UpdateNotes(id, notes);
            return Ok();
        }
        [HttpPost("updateIsUpdated")]
        public async Task<IActionResult> UpdateIsUpdated([FromQuery] int id, [FromBody] bool isUpdated)
        {
            await _serialService.UpdateIsUpdated(id, isUpdated);
            return Ok();
        }
        [HttpGet("getSerials")]
        public async Task<IActionResult> GetSerials([FromQuery] QueryStringParameters queryStringParameters, [FromQuery] IEnumerable<int> platforms)
        {
            var serials = await _serialService.GetByPage(queryStringParameters, platforms);
            return Ok(serials);
        }
        [HttpPost("setPlatformsByNames")]
        public async Task<IActionResult> SetPlatformsByNames([FromBody] IEnumerable<CustomPlatform> platforms, int id)
        {
            await _serialService.SetPlatformsByNames(platforms, id);
            return Ok();
        }
        [HttpGet("getSerialByUrl")]
        public async Task<IActionResult> GetSerialByUrl(string url)
        {
            return Ok(await _serialService.GetByUrl(url));
        }

        [HttpPost("editSerial")]
        public async Task<IActionResult> EditSerial(EditSerialDTO serialDTO)
        {
            if (serialDTO == null) throw new HttpException("SerialDTO is null!", System.Net.HttpStatusCode.BadRequest);
            await _serialService.Edit(serialDTO);
            return Ok();
        }
        [HttpPost("addSerial")]
        public async Task<IActionResult> AddSerial(SerialDTO serialDTO)
        {
            await _serialService.Add(serialDTO);
            return Ok();
        }
    }
}
