using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.General;

namespace Core.DTOs.Response
{
    public class MoviesResponseDTO
    {
        public IEnumerable<MovieDTO> Items { get; set; }
        public PaginationMetadata Metadata { get; set; }

    }
}
