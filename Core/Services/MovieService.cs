using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Classes;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.CustomServices;

namespace Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public async Task Add(MovieDTO movieDTO)
        {
            await _unitOfWork.MovieRepository.Insert(_mapper.Map<Movie>(movieDTO));
            await _unitOfWork.MovieRepository.SaveChangesAsync();
        }

        public PagedList<MovieDTO> GetByPage(QueryStringParameters queryStringParameters)
        {
            //return
            var collection = _mapper.Map<IEnumerable<MovieDTO>>(_unitOfWork.MovieRepository.GetByPage(queryStringParameters)).ToList();
            return new PagedList<MovieDTO>(collection, collection.Count, queryStringParameters.PageNumber, queryStringParameters.PageSize);
        }

        public async Task<MovieDTO> GetByUrl(string url)
        {
            var movie = (await _unitOfWork.MovieRepository.Get(el => el.Url == url)).FirstOrDefault();
            return movie == null ? null : _mapper.Map<MovieDTO>(movie);
        }

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
