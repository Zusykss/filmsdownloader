﻿using Core.Classes;
using Core.DTOs;
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
        public IActionResult GetMovies([FromQuery] QueryStringParameters queryStringParameters)
        {
            var movies = _movieService.GetByPage(queryStringParameters);
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

        [HttpPost("addMovie")]
        public async Task<IActionResult> AddMovie(MovieDTO movieDTO)
        {
            await _movieService.Add(movieDTO);
            return Ok();
        }

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
    }
}