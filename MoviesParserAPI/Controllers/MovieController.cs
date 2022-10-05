using Core.Classes;
using Core.DTOs;
using Core.DTOs.Edit;
using Core.Entities;
using Core.Interfaces.CustomServices;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MoviesParserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromQuery] int id, [FromQuery] int statusId)
        {
            await _movieService.UpdateStatus(id, statusId);
            return Ok(); 
        }
        [HttpPost("updateNotes")]
        public async Task<IActionResult> UpdateNotes([FromQuery] int id, [FromBody] string notes)
        {
            await _movieService.UpdateNotes(id, notes);
            return Ok();
        }

        [HttpGet("getMovies")]
        public async Task<IActionResult> GetMovies([FromQuery] QueryStringParameters queryStringParameters,[FromQuery] IEnumerable<int> platforms )
        {
            var movies = await _movieService.GetByPage(queryStringParameters, platforms);
            return Ok(movies); //.Items
        }

        [HttpPost("editMovie")]
        public async Task<IActionResult> UpdateMovie([FromBody]EditMovieDTO movieDTO)
        {
            await _movieService.Edit(movieDTO);
            return Ok();
        }
        [HttpPost("setPlatformsByNames")]
        public async Task<IActionResult> SetPlatformsByNames([FromBody]IEnumerable<CustomPlatform> platforms, int id)
        {
            await _movieService.SetPlatformsByNames(platforms, id);
            return Ok();
        }
        [HttpPost("addMovie")]
        public async Task<IActionResult> AddMovie([FromBody] MovieDTO movieDTO)
        {
            await _movieService.Add(movieDTO);
            return Ok();
        }

        [HttpGet("getMovieByUrl")]
        public async Task<IActionResult> GetMovieByUrl(string url)
        {
            return Ok(await _movieService.GetByUrl(url));
        }
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
    }
}
