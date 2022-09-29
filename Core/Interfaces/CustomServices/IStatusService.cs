using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;

namespace Core.Interfaces.CustomServices
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusDTO>> GetStatuses();
        Task<StatusDTO> GetStatus(int id);
    }
}
