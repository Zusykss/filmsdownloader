using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Classes;
using Core.DTOs;
using Core.DTOs.Edit;
using Core.DTOs.General;
using Core.DTOs.Response;
using Core.Entities;
using Core.Exceptions;
using Core.Helpers.Extensions;
using Core.Helpers.Options;
using Core.Interfaces;
using Core.Interfaces.CustomServices;
using Microsoft.Extensions.Options;

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

        public async Task<MoviesResponseDTO> GetByPage(QueryStringParameters queryStringParameters,
            IEnumerable<int> platforms)
        {
            //return
            //(await _unitOfWork.MovieRepository.Get()).Include
            var collection = _mapper.Map<IEnumerable<MovieDTO>>((await _unitOfWork.MovieRepository.Get(includeProperties: "Status,PlatformsMovies"))).AsQueryable();////GetByPage(queryStringParameters)).ToList(); //.OrderByDescending(m => m.ParseTime)
            if (!string.IsNullOrEmpty(queryStringParameters.QuerySearch))
            {
                collection = collection.Where(m => m.Name.Contains(queryStringParameters.QuerySearch) || m.Url.Contains(queryStringParameters.QuerySearch));
            }

            var platformsList = platforms.ToList();//platforms.ToArray();
            if (platformsList.Any())
            {
                collection = collection.Where((m) => m.PlatformsMovies.Any() && m.PlatformsMovies.Any((pm) => platforms.Contains(pm.Platform.Id)
               ));

                // {
                //    if (platformsList.Contains(pm.Platform.Id))
                //    {
                //        platformsList.Remove(pm.Platform.Id);
                //        return true;
                //    }
                //    //else if(!platformsList.Any()) return true;
                //    return false;
                //    ;
                //}
            }

            if (!String.IsNullOrWhiteSpace(queryStringParameters.OrderProperty))
            {
                bool ascending = !(!String.IsNullOrWhiteSpace(queryStringParameters.OrderBy) && queryStringParameters.OrderBy == "desc");
                collection = collection.OrderByPropertyName(queryStringParameters.OrderProperty, ascending);
            }
            //    var orderParams = queryStringParameters.OrderBy.Trim().Split(',');
            //    var propertyInfos = typeof(MovieDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //    var orderQueryBuilder = new StringBuilder();
            //    foreach (var param in orderParams)
            //    {
            //        if (string.IsNullOrWhiteSpace(param))
            //            continue;
            //        var propertyFromQueryName = param.Split(" ")[0];
            //        var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            //        if (objectProperty == null)
            //            continue;
            //        var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
            //        orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            //    }
            //    var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            //    IQueryable<MovieDTO> query = collection.Where(el => el.Name == "dd");
            //    collection = query.Sort(orderQuery);
            //else
            //{
            //    collection = collection.OrderBy(x => x.Name);
            //}
            // Get's No of Rows Count   
            int count = collection.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = queryStringParameters.PageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = queryStringParameters.PageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = collection.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new PaginationMetadata
            {
                TotalCount = TotalCount,
                PageSize = PageSize,
                CurrentPage = CurrentPage,
                TotalPages = TotalPages,
                PreviousPage = previousPage,
                NextPage = nextPage,
                QuerySearch = string.IsNullOrEmpty(queryStringParameters.QuerySearch) ?
                    "No Parameter Passed" : queryStringParameters.QuerySearch
            };

            // Setting Header  
            
            return new MoviesResponseDTO{ Items = items, Metadata = paginationMetadata };
            //return new PagedList<MovieDTO>(collection, collection.Count, queryStringParameters.PageNumber, queryStringParameters.PageSize);
        }

        public async Task<MovieDTO> GetByUrl(string url)
        {
            var movie = (await _unitOfWork.MovieRepository.Get(el => el.Url == url)).FirstOrDefault();
            return movie == null ? null : _mapper.Map<MovieDTO>(movie);
        }

        public async Task UpdateStatus(int id, int statusId)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            if (movie == null) throw new HttpException("Movie doesn`t exists", HttpStatusCode.BadRequest);
            var status = await _unitOfWork.StatusRepository.GetById(statusId);
            if (status == null) throw new HttpException("Status doesn`t exists", HttpStatusCode.BadRequest);
            movie.Status = status;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Edit(EditMovieDTO movieDTO)
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
            var movie = _mapper.Map<Movie>(movieDTO);
            _unitOfWork.MovieRepository.Update(movie); //D
            await _unitOfWork.MovieRepository.SaveChangesAsync();
        }

        public async Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            movie.PlatformsMovies.Clear();
            var platformsEntities = await _unitOfWork.PlatformRepository.Get();
            foreach (var platform in platforms)
            {
                var pl = platformsEntities.FirstOrDefault(el => el.Name == platform.Name);
                if (pl != null)
                {
                    await _unitOfWork.PlatformMovieRepository.Insert(new PlatformMovie { Movie = movie, Platform = pl, Url = platform.Url});
                }
            }
           // var platformsModels = (await _unitOfWork.PlatformRepository.Get(el => platforms.Any(p => p.Name == el.Name))).ToList();
            //foreach (var plat in platformsModels)
            //{
            //    await _unitOfWork.PlatformMovieRepository.Insert(new PlatformMovie{ Movie = movie, Platform = plat, Url = plat.});
            //}
            //_unitOfWork.MovieRepository.Update(movie);
            //await _unitOfWork.SaveChangesAsync();
            //movie.PlatformsMovies =
            // 
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateNotes(int id, string notes)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            if (movie == null) throw new HttpException("Movie doesn`t exists", HttpStatusCode.BadRequest);
            movie.Notes = notes;
            await _unitOfWork.MovieRepository.SaveChangesAsync();
        }


        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
