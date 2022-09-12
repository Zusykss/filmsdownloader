using System;
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
    public class SerialService : ISerialService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public async Task Add(SerialDTO serialDTO)
        {
            await _unitOfWork.SerialRepository.Insert(_mapper.Map<Serial>(serialDTO));
        }
        
        public PagedList<SerialDTO> GetByPage(QueryStringParameters queryStringParameters)
        {
            throw new NotImplementedException();
        }
        public SerialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
