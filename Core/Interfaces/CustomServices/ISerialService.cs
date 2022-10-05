using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes;
using Core.DTOs;
using Core.DTOs.Edit;
using Core.DTOs.Response;

namespace Core.Interfaces.CustomServices
{
    public interface ISerialService
    {
        Task Add(SerialDTO movieDTO);
        Task<SerialsResponseDTO> GetByPage(QueryStringParameters queryStringParameters, IEnumerable<int> platforms);
        Task<SerialDTO> GetByUrl(string url);
        Task Edit(EditSerialDTO serialDTO);
        Task SetPlatformsByNames(IEnumerable<CustomPlatform> platforms, int id);
        Task UpdateNotes(int id, string notes);
        Task UpdateIsUpdated(int id, bool isUpdated);
        Task UpdateStatus(int id, int statusId);
    }
}
