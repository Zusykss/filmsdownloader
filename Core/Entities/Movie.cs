using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public int StatusId { get; set; } = 3;
        public virtual Status Status { get; set; }
        public DateTime ParseTime { get; set; } = DateTime.Now;
        public virtual ICollection<PlatformMovie> PlatformsMovies { get; set; } = new HashSet<PlatformMovie>();

    }
}
