using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Serial
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public string Seasons { get; set; }
        public string Series { get; set; }
        public bool IsUpdated { get; set; } = false;
        public int StatusId { get; set; } = 3;
        public virtual Status Status { get; set; }
        public DateTime ParseTime { get; set; } = DateTime.Now;
        public virtual ICollection<PlatformSerial> PlatformsSerials { get; set; } = new HashSet<PlatformSerial>();
    }
}
