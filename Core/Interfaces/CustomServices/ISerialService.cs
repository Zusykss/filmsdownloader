using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Classes;
using Core.DTOs;

namespace Core.Interfaces.CustomServices
{
    public interface ISerialService
    {
        Task Add(SerialDTO movieDTO);
        PagedList<SerialDTO> GetByPage(QueryStringParameters queryStringParameters);
        Task<SerialDTO> GetByUrl(string url);
        Task Edit(SerialDTO serialDTO);
    }
}
