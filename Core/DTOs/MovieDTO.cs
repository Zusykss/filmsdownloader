using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class MovieDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
    }
}
