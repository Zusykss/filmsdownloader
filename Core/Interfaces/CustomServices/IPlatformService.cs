﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;

namespace Core.Interfaces.CustomServices
{
    public interface IPlatformService
    {
        Task AddIfNotExists(PlatformDTO platformDTO);
        Task<IEnumerable<PlatformDTO>> GetAllPlatforms();
        Task<PlatformDTO> GetPlatformById(int id);
    }
}
