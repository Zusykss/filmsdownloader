using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes;
using Core.DTOs;
using Core.DTOs.Edit;
using Core.DTOs.Response;

namespace Core.Interfaces.CustomServices
{
    public interface IMovieService
    {
        Task Add(MovieDTO movieDTO);
        Task<MoviesResponseDTO> GetByPage(QueryStringParameters queryStringParameters, IEnumerable<int> platforms);
        Task<MovieDTO> GetByUrl(string url);
        Task UpdateStatus(int id, int statusId);
        Task Edit(EditMovieDTO movieDTO);
        Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id);
        Task UpdateNotes(int id, string notes);
    }
}
