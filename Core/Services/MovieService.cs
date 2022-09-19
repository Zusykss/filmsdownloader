using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Classes;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
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

        public async Task Edit(MovieDTO movieDTO)
        {
            if (movieDTO == null || !movieDTO.Id.HasValue)
            {
                throw new HttpException("Incorrect movie data!", System.Net.HttpStatusCode.BadRequest);
            }

            //if (!movieDTO.Id.HasValue)
            //{
            //    var movieId = (_unitOfWork.MovieRepository.GetFirstAsNoTracking(el => el.Url == movieDTO.Url))?.Id;
            //    if (!movieId.HasValue) throw new HttpException("Incorrect url (doesn`t exist)", HttpStatusCode.BadRequest);
            //    movieDTO.Id = movieId.Value;
            //}
            _unitOfWork.MovieRepository.Update(_mapper.Map<Movie>(movieDTO)); //D
            await _unitOfWork.MovieRepository.SaveChangesAsync();
        }

        public async Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            movie.PlatformsMovies.Clear(); 
            var platformsModels = (await _unitOfWork.PlatformRepository.Get(el => platforms.Any(p => p.Name == el.Name))).ToHashSet();
            foreach (var plat in platformsModels)
            {
                await _unitOfWork.PlatformMovieRepository.Insert(new PlatformMovie{ Movie = movie, Platform = plat});
            }
            //_unitOfWork.MovieRepository.Update(movie);
            //await _unitOfWork.SaveChangesAsync();
            //movie.PlatformsMovies =
            // 
            await _unitOfWork.SaveChangesAsync();
        }


        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
