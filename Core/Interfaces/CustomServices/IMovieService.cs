using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes;
using Core.DTOs;
using Core.DTOs.Response;

namespace Core.Interfaces.CustomServices
{
    public interface IMovieService
    {
        Task Add(MovieDTO movieDTO);
        Task<MoviesResponseDTO> GetByPage(QueryStringParameters queryStringParameters);
        Task<MovieDTO> GetByUrl(string url);
        Task Edit(MovieDTO movieDTO);
        Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id);
    }
}
