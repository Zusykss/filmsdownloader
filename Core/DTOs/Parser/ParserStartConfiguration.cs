using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Parser
{
    public class ParserStartConfiguration
    {
        public int? Count { get; set; }
        //[JsonProperty("category")]
        public int Category { get; set; }
        public int[] Platforms { get; set; }
    }

    public enum ParserStartCategory
    {
        Film = 1,
        Serial = 2
    }
}
