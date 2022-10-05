using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Edit
{
    public class EditSerialDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Seasons { get; set; }
        public string Series { get; set; }
        public string Notes { get; set; }

        public bool IsUpdated { get; set; }
    }
}
