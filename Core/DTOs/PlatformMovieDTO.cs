using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PlatformMovieDTO
    {
        public string Url { get; set; }
        public PlatformDTO Platform { get; set; }
    }
}
