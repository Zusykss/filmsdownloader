using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.CustomServices;

namespace Core.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public async Task AddIfNotExists(PlatformDTO platformDTO)
        {
            if (await _unitOfWork.PlatformRepository.GetFirst(el => el.Name == platformDTO.Name) == null)
            {
                await _unitOfWork.PlatformRepository.Insert(_mapper.Map<Platform>(platformDTO));
                await _unitOfWork.PlatformRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PlatformDTO>> GetAllPlatforms()
        {
            return  (_mapper.Map<IEnumerable< PlatformDTO>>(await _unitOfWork.PlatformRepository.Get()));
        }

        public async Task<PlatformDTO> GetPlatformById(int id) => _mapper.Map<PlatformDTO>(await _unitOfWork.PlatformRepository.GetById(id));

        public PlatformService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
