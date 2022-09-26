using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Parser;
using Core.Helpers.Options;
using Core.Interfaces.CustomServices;
using Microsoft.Extensions.Options;

namespace Core.Services
{
    public class ParserService : IParserService
    {
        private readonly IOptions<ParserSettings> _parserSettings;

        public ParserService(IOptions<ParserSettings> parserSettings)
        {
            _parserSettings = parserSettings;
        }

        public ParserState GetParserState()
        {
            return Process.GetProcessesByName("MoviesParser").Length > 0 ? ParserState.Started : ParserState.Closed;
        }
        public void StopParser()
        {
            Process[] workers = Process.GetProcessesByName("MoviesParser");
            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
            workers = Process.GetProcessesByName("chrome");
            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
        }
        public void StartParser(ParserStartConfiguration parserStartConfiguration)
        {
            StringBuilder arguments = new StringBuilder();
            //if (parserStartConfiguration.)
            //{
                
            //}
            arguments.Append("-category=");
            arguments.Append(parserStartConfiguration.Category);
            if (parserStartConfiguration.Platforms.Length > 0)
            {
                arguments.Append(" -platforms=\"");
                arguments.Append(String.Join(',', parserStartConfiguration.Platforms));
                arguments.Append('"');
            }
            if (parserStartConfiguration.Count.HasValue)
            {
                arguments.Append(" -count=");
                arguments.Append(parserStartConfiguration.Count);
            }

            var process = new Process
            {
                StartInfo =
                {
                    FileName = _parserSettings.Value.AppPath + "/" + _parserSettings.Value.FileName,
                    WorkingDirectory = _parserSettings.Value.AppPath,
                    UseShellExecute = true,
                    Arguments = arguments.ToString()
                }
            };
            process.Start();
        }
    }
}
