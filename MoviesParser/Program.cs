#region Usings
using MoviesParser;
#endregion



var category = File.ReadAllLines("settings.txt")[5];
PuppeteerWorker puppeteerWorker = new PuppeteerWorker(category);

await puppeteerWorker.Start();