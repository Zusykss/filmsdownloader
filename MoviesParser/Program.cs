#region Usings
using MoviesParser;
#endregion

var category = "movie";
PuppeteerWorker puppeteerWorker = new PuppeteerWorker(category);

await puppeteerWorker.Start();