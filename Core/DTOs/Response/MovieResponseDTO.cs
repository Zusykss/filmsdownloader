using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Response
{
    public class MovieResponseDTO
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string[] Platforms { get; set; }
    }
}
