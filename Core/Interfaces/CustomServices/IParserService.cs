using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Parser;

namespace Core.Interfaces.CustomServices
{
    public interface IParserService
    {
        void StartParser(ParserStartConfiguration parserStartConfiguration);
        public ParserState GetParserState();
        public void StopParser();
    }
}
