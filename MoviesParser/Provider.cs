using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesParser
{
    public class Provider
    {
        private Provider(string value)
        {
            Value = value;
        }
        public string Value { get; private set; }
        public static Provider Netflix => new Provider("/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg");
    }
}
