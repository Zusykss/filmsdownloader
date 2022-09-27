#region Usings
using MoviesParser;
using MoviesParser.Helpers;

#endregion



//var category = File.ReadAllLines("settings.txt")[5];
Console.WriteLine("  __  __            _        _____                         \r\n |  \\/  |          (_)      |  __ \\                        \r\n | \\  / | _____   ___  ___  | |__) |_ _ _ __ ___  ___ _ __ \r\n | |\\/| |/ _ \\ \\ / / |/ _ \\ |  ___/ _` | '__/ __|/ _ \\ '__|\r\n | |  | | (_) \\ V /| |  __/ | |  | (_| | |  \\__ \\  __/ |   \r\n |_|  |_|\\___/ \\_/ |_|\\___| |_|   \\__,_|_|  |___/\\___|_|   \r\n                                                           ");
Console.WriteLine("Started");
//Console.WriteLine(String.Join(' ',args));
//Console.WriteLine(args.First());

ParserStartConfiguration configuration = new ParserStartConfiguration();
var category = args[0];
configuration.ParserStartCategory = category.Substring(category.IndexOf('=') + 1) == "1" ? "movie" : "tv";
    //var platformsArgs = 
if (args.Length > 1 && args[1].Contains("platforms"))
{
    var platforms = args[1].Substring(args[1].IndexOf('=') + 1).Split(',').Select(p => int.Parse(p)).ToArray();
    configuration.Platforms = platforms;
}
if (args.Length > 2 && args[2].Contains("count"))
{
    configuration.Count = int.Parse(args[2].Substring(args[2].IndexOf('=') + 1));
}
//if (.Contains("category"))
//{
//}
PuppeteerWorker puppeteerWorker = new PuppeteerWorker(configuration);//category
//args.
await puppeteerWorker.Start();