using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.CustomServices;

namespace Core.Services
{
    public class StatusService: IStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StatusDTO>> GetStatuses()
        {
            return _mapper.Map<IEnumerable<StatusDTO>>(await _unitOfWork.StatusRepository.Get());
        }

        public async Task<StatusDTO> GetStatus(int id)
        {
            return  _mapper.Map<StatusDTO>(await _unitOfWork.StatusRepository.GetById(id));
        }
    }
}
