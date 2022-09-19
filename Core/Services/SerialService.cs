using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SerialService : ISerialService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public async Task Add(SerialDTO serialDTO)
        {
            await _unitOfWork.SerialRepository.Insert(_mapper.Map<Serial>(serialDTO));
            await _unitOfWork.SaveChangesAsync();
        }
        
        public PagedList<SerialDTO> GetByPage(QueryStringParameters queryStringParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<SerialDTO> GetByUrl(string url)
        {
            var serial = (await _unitOfWork.SerialRepository.Get(el => el.Url == url)).FirstOrDefault();
            return serial == null ? null : _mapper.Map<SerialDTO>(serial);
        }

        public async Task Edit(SerialDTO serialDTO)
        {
            if (serialDTO == null || !serialDTO.Id.HasValue)
            {
                throw new HttpException("Incorrect serial data!", System.Net.HttpStatusCode.BadRequest);
            }
            _unitOfWork.MovieRepository.Update(_mapper.Map<Movie>(serialDTO)); //D
            await _unitOfWork.SerialRepository.SaveChangesAsync();
        }

        public async Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id)
        {
            var serial = await _unitOfWork.SerialRepository.GetById(id);
            serial.PlatformsSerials.Clear();
            var platformsModels = (await _unitOfWork.PlatformRepository.Get(el => platforms.Any(p => p.Name == el.Name))).ToHashSet();
            foreach (var plat in platformsModels)
            {
                await _unitOfWork.PlatformSerialRepository.Insert(new PlatformSerial { Serial = serial, Platform = plat });
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public SerialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
