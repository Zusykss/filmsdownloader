#region Usings

using System.Net;
using MoviesParser;
using MoviesParser.Helpers;

#endregion



//var category = File.ReadAllLines("settings.txt")[5];
ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
Console.WriteLine("  __  __            _        _____                         \r\n |  \\/  |          (_)      |  __ \\                        \r\n | \\  / | _____   ___  ___  | |__) |_ _ _ __ ___  ___ _ __ \r\n | |\\/| |/ _ \\ \\ / / |/ _ \\ |  ___/ _` | '__/ __|/ _ \\ '__|\r\n | |  | | (_) \\ V /| |  __/ | |  | (_| | |  \\__ \\  __/ |   \r\n |_|  |_|\\___/ \\_/ |_|\\___| |_|   \\__,_|_|  |___/\\___|_|   \r\n                                                           ");
Console.WriteLine("Started");
//Console.WriteLine(String.Join(' ',args));
//Console.WriteLine(args.First());

ParserStartConfiguration configuration = new ParserStartConfiguration();
string category = args.FirstOrDefault(el => el.Contains("-category"));
category = category != null ? category.Substring(category.IndexOf('=') + 1) == "1" ? "movie" : "tv" : "tv";
configuration.ParserStartCategory = category;
    //var platformsArgs = 

string platforms = args.FirstOrDefault(el => el.Contains("-platforms"));
if (platforms != null)
{
    configuration.Platforms = platforms.Substring(args[1].IndexOf('=') + 1).Split(',').Select(p => int.Parse(p)).ToArray();
}
//if (args.Length > 1 && args[1].Contains("platforms"))
//{
//    var platforms = args[1].Substring(args[1].IndexOf('=') + 1).Split(',').Select(p => int.Parse(p)).ToArray();
//    configuration.Platforms = platforms;
//}
string count = args.FirstOrDefault(el => el.Contains("-count"));
if (count != null)
{
    configuration.Count = int.Parse(count.Substring(count.IndexOf('=') + 1));
}
//if (args.Length > 2 && args[2].Contains("count"))
//{
//}
//if (.Contains("category"))
//{
//}
PuppeteerWorker puppeteerWorker = new PuppeteerWorker(configuration);//category
//args.
await puppeteerWorker.Start();