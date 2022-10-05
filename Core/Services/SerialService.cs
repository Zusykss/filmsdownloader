using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Core.Interfaces;
using Core.Interfaces.CustomServices;

namespace Core.Services
{
    public class SerialService : ISerialService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public async Task Add(SerialDTO serialDTO)
        {
            await _unitOfWork.SerialRepository.Insert(_mapper.Map<Serial>(serialDTO));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<SerialsResponseDTO> GetByPage(QueryStringParameters queryStringParameters, IEnumerable<int> platforms)
        {
            var collection = _mapper.Map<IEnumerable<SerialDTO>>((await _unitOfWork.SerialRepository.Get())).AsQueryable();////GetByPage(queryStringParameters)).ToList(); //.OrderByDescending(m => m.ParseTime)
            if (!string.IsNullOrEmpty(queryStringParameters.QuerySearch))
            {
                collection = collection.Where(m => m.Name.Contains(queryStringParameters.QuerySearch) || m.Url.Contains(queryStringParameters.QuerySearch));
            }

            var platformsList = platforms.ToList();//platforms.ToArray();
            if (platformsList.Any())
            {
                collection = collection.Where((m) => m.PlatformsSerials.Any() && m.PlatformsSerials.Any((pm) => platforms.Contains(pm.Platform.Id)
               ));
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

            // Setting Response  

            return new SerialsResponseDTO { Items = items, Metadata = paginationMetadata };
        }


        public async Task<SerialDTO> GetByUrl(string url)
        {
            var serial = (await _unitOfWork.SerialRepository.Get(el => el.Url == url)).FirstOrDefault();
            return serial == null ? null : _mapper.Map<SerialDTO>(serial);
        }

        public async Task Edit(EditSerialDTO serialDTO)
        {
            if (serialDTO == null || !serialDTO.Id.HasValue)
            {
                throw new HttpException("Incorrect serial data!", System.Net.HttpStatusCode.BadRequest);
            }
            _unitOfWork.SerialRepository.Update(_mapper.Map<Serial>(serialDTO)); //D
            await _unitOfWork.SerialRepository.SaveChangesAsync();
        }

        public async Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id)
        {
            var serial = await _unitOfWork.SerialRepository.GetById(id);
            serial.PlatformsSerials.Clear();
            //var platformsModels = (await _unitOfWork.PlatformRepository.Get(el => platforms.Any(p => p.Name == el.Name))).ToHashSet();
            //foreach (var plat in platformsModels)
            //{
            //    await _unitOfWork.PlatformSerialRepository.Insert(new PlatformSerial { Serial = serial, Platform = plat });
            //}
            var platformsEntities = await _unitOfWork.PlatformRepository.Get();
            foreach (var platform in platforms)
            {
                var pl = platformsEntities.FirstOrDefault(el => el.Name == platform.Name);
                if (pl != null)
                {
                    await _unitOfWork.PlatformSerialRepository.Insert(new PlatformSerial { Serial = serial, Platform = pl, Url = platform.Url });
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateNotes(int id, string notes)
        {
            var serial = await _unitOfWork.SerialRepository.GetById(id);
            if (serial == null) throw new HttpException("Movie doesn`t exists", HttpStatusCode.BadRequest);
            serial.Notes = notes;
            await _unitOfWork.SerialRepository.SaveChangesAsync();
        }

        public async Task UpdateIsUpdated(int id, bool isUpdated)
        {
            var serial = await _unitOfWork.SerialRepository.GetById(id);
            if (serial == null) throw new HttpException("Movie doesn`t exists", HttpStatusCode.BadRequest);
            serial.IsUpdated = isUpdated;
            await _unitOfWork.SerialRepository.SaveChangesAsync();
        }

        public async Task UpdateStatus(int id, int statusId)
        {
            var serial = await _unitOfWork.SerialRepository.GetById(id);
            if (serial == null) throw new HttpException("Movie doesn`t exists", HttpStatusCode.BadRequest);
            var status = await _unitOfWork.StatusRepository.GetById(statusId);
            if (status == null) throw new HttpException("Status doesn`t exists", HttpStatusCode.BadRequest);
            serial.Status = status;
            await _unitOfWork.SaveChangesAsync();
        }

        public SerialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
