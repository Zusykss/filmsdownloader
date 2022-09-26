using Core.Classes;
using Core.DTOs;
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

        [HttpGet("getMovies")]
        public async Task<IActionResult> GetMovies([FromQuery] QueryStringParameters queryStringParameters)
        {
            var movies = await _movieService.GetByPage(queryStringParameters);
            //var metadata = new
            //{
            //    movies.TotalCount,
            //    movies.PageSize,
            //    movies.CurrentPage,
            //    movies.TotalPages,
            //    movies.HasNext,
            //    movies.HasPrevious
            //};
            //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(movies.Metadata));
            return Ok(movies.Items);
        }

        [HttpPost("editMovie")]
        public async Task<IActionResult> UpdateMovie(MovieDTO movieDTO)
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
