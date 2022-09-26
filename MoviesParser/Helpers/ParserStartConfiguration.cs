using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesParser.Helpers
{
    public class ParserStartConfiguration
    {
        public int? Count { get; set; }
            public string ParserStartCategory { get; set; }
            public int[] Platforms { get; set; }
        }
}
