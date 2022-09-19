using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        // public virtual ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
        //public virtual ICollection<Serial> Serials { get; set; } = new HashSet<Serial>();
        public virtual ICollection<PlatformSerial> PlatformsSerials { get; set; } = new HashSet<PlatformSerial>();
        public virtual ICollection<PlatformMovie> PlatformsMovies { get; set; } = new HashSet<PlatformMovie>();
    }
}
