using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes;
using Core.DTOs;

namespace Core.Interfaces.CustomServices
{
    public interface IMovieService
    {
        Task Add(MovieDTO movieDTO);
        PagedList<MovieDTO> GetByPage(QueryStringParameters queryStringParameters);
    }
}
