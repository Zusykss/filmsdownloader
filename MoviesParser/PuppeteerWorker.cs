using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MoviesParser.DTO_s;
using MoviesParser.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Page = PuppeteerSharp.Page;

namespace MoviesParser
{
    public class PuppeteerWorker
    {
        private readonly ParserStartConfiguration _configuration;
        private int? maxCount = null;
        private int counter = 0;
        private Browser _browser;
        private Page _page;
        private Page _tmpPage;
        //private Page _seriesPage;
        private LaunchOptions _options;
        //private string _category;
        int rowIndex = 1;
        int pageIndex = 1;
        private FileInfo _filePath;
        private string _proxy = File.ReadAllLines("settings.txt")[2];
        private Dictionary<string, string> _providers;

        //Color colFromHex = new Color{ Rgb = ""}//System.Drawing.ColorTranslator.FromHtml("#B7DEE8");

        private void CreateExcelFileIfIsNotExists()
        {
            //_filePath = 
            string path = File.ReadAllLines("settings.txt")[1];
            if (!File.Exists(path))
            {
                ExcelPackage ExcelPkg = new ExcelPackage();
                ExcelWorksheet wsSheet = ExcelPkg.Workbook.Worksheets.Add("List1");
                wsSheet.Protection.IsProtected = false;
                wsSheet.Protection.AllowSelectLockedCells = false;
                ExcelPkg.SaveAs(new FileInfo(path));
            }
            _filePath = new FileInfo(path);


        }

        public PuppeteerWorker(ParserStartConfiguration configuration)//string category = "movie"
        {
            _configuration = configuration;
            _options = new LaunchOptions
            {
                Headless = false,
                Args = new[]{
                    "--disable-gpu",
                    "--disable-dev-shm-usage",
                    "--disable-setuid-sandbox",
                    "--no-first-run",
                    "--no-sandbox",
                    "--no-zygote",
                    "--deterministic-fetch",
                    "--disable-features=IsolateOrigins",
                    "--disable-site-isolation-trials",
                    $"--proxy-server=http://{_proxy}",
                    "--lang=en-GB"
                  },
                IgnoredDefaultArgs = new string[]
                {
                    "--enable-automation"
                },
                ExecutablePath = File.ReadAllLines("settings.txt")[0]

            };
            //_category = category;
            //_filePath = new FileInfo(File.ReadAllLines("settings.txt")[1]);
            // Ставимо ліцензію (не комерційну) на OfficeOpenXml
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            _providers = new Dictionary<string, string>();
            #region providers
            //{
            //    {"Netflix", "/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg"},
            //    {"Prime Video", "/t/p/original/emthp39XA2YScoYL1p0sdbAH2WA.jpg"},
            //    {"Apple iTunes", "/t/p/original/peURlLlr8jggOwK53fJ5wdQl05y.jpg"},
            //    {"Google Play Movies", "/t/p/original/tbEdFQDwx5LEVr8WpSeXQSIirVq.jpg"},
            //    {"Sun Nxt", "/t/p/original/uW4dPCcbXaaFTyfL5HwhuDt5akK.jpg"},
            //    {"Mubi", "/t/p/original/bVR4Z1LCHY7gidXAJF5pMa4QrDS.jpg"},
            //    {"Looke", "/t/p/original/mPDlxHokGsEc84OOhp9qjeynq2U.jpg"},
            //    {"Clasix", "/t/p/original/iaMw6nOyxUzXSacrLQ0Au6CfZkc.jpg"},
            //    {"Star Plus", "/t/p/original/hR9vWd8hWEVQKD6eOnBneKRFEW3.jpg"},
            //    {"Paramount Plus", "/t/p/original/xbhHHa1YgtpwhC8lb1NQ3ACVcLd.jpg"},
            //    {"HBO Max", "/t/p/original/Ajqyt5aNxNGjmF9uOfxArGrdf3X.jpg"},
            //    {"Crunchyroll", "/t/p/original/8Gt1iClBlzTeQs8WQm8UrCoIxnQ.jpg"},
            //    {"Cultpix", "/t/p/original/59azlQKUgFdYq6QI5QEAxIeecyL.jpg"},
            //    {"Claro video", "/t/p/original/lJT7r1nprk1Z8t1ywiIa8h9d3rc.jpg"},
            //    {"Globoplay", "/t/p/original/oBoWstXQFHAlPApyxIQ31CIbNQk.jpg"},
            //    {"Apple TV Plus", "/t/p/original/6uhKBfmtzFqOcLousHwZuzcrScK.jpg"},
            //    {"Amazon Video", "/t/p/original/5NyLm42TmCqCMOZFvH4fcoSNKEW.jpg"},
            //    {"FilmBox+", "/t/p/original/4FqTBYsUSZgS9z9UGKgxSDBbtc8.jpg"},
            //    {"Vix", "/t/p/original/58aUMVWJRolhWpi4aJCkGHwfKdg.jpg"},
            //    {"Curiosity Stream", "/t/p/original/67Ee4E6qOkQGHeUTArdJ1qRxzR2.jpg"},
            //    {"Kocowa", "/t/p/original/zyX0rRd986t2iKXUCvEsW7or4KN.jpg"},
            //    {"Funimation Now", "/t/p/original/o252SN51PdMx8UvyUkX00MAtooX.jpg"},
            //    {"NetMovies", "/t/p/original/rll0yTCjrSY6hcJqIyMatv9B2iR.jpg"},
            //    {"Starz Play Amazon Channel", "/t/p/original/x36C6aseF5l4uX99Kpse9dbPwBo.jpg"},
            //    {"WOW Presents Plus","/t/p/original/mgD0T960hnYU4gBxbPPBrcDfgWg.jpg"},
            //    {"Magellan TV", "/t/p/original/gekkP93StjYdiMAInViVmrnldNY.jpg"},
            //    {"Paramount+ Amazon Channel", "/t/p/original/3E0RkIEQrrGYazs63NMsn3XONT6.jpg"},
            //    {"BroadwayHD","/t/p/original/xLu1rkZNOKuNnRNr70wySosfTBf.jpg"},
            //    {"Believe", "/t/p/original/dFnG5G2YxrYjv9YiVu9Bq7Wj5Ds.jpg"},
            //    {"Filmzie", "/t/p/original/olmH7t5tEng8Yuq33KmvpvaaVIg.jpg"},
            //    {"Dekkoo", "/t/p/original/u2H29LCxRzjZVUoZUQAHKm5P8Zc.jpg"},
            //    {"Belas Artes à La Carte", "/t/p/original/fy4svqyray3cnkuEqGIXL3i2WQw.jpg"},
            //    {"True Story", "/t/p/original/osREemsc9uUB2J8VTkQeAVk2fu9.jpg"},
            //    {"HBO Go", "/t/p/original/bmU37kpSMbcTgwwUrbxByk7x8h3.jpg"},
            //    {"DocAlliance Films", "/t/p/original/aQ1ritN00jXc7RAFfUoQKGAAfp7.jpg"},
            //    {"Hoichoi", "/t/p/original/d4vHcXY9rwnr763wQns2XJThclt.jpg"},
            //    {"KoreaOnDemand", "/t/p/original/uHv6Y4YSsr4cj7q4cBbAg7WXKEI.jpg"},
            //    {"NOW", "/t/p/original/cQQYtdaCg7vDo28JPru4v8Ypi8x.jpg"},
            //    {"Pluto TV", "/t/p/original/t6N57S17sdXRXmZDAkaGP0NHNG0.jpg"},
            //    {"Oldflix", "/t/p/original/1bbExrGyEuUFAEWMBSN76bwacQ0.jpg"},
            //    {"TNTGo", "/t/p/original/dUokaRky9vs1u2PFRzFDV4iIx6A.jpg"},
            //    {"Disney Plus", "/t/p/original/7rwgEs15tFwyR9NPQ5vpzxTj19Q.jpg"},
            //    {"GOSPEL PLAY", "/t/p/original/plbVK1EXpz7PpyddpI0U1cZIYYK.jpg"},
            //    {"Libreflix", "/t/p/original/n3BIqc0mojP85bJSKjsIwZUOVya.jpg"},
            //    {"Supo Mungam Plus", "/t/p/original/rWYJ9mMvxs0p57Nd1BKEtKtpRvD.jpg"},
            //    {"Filme Filme", "/t/p/original/qEFO4pJhL6IyHbKUqaefsOA0kWJ.jpg"},
            //    {"Starz" , "/t/p/original/zVJhpmIEgdDVbDt5TB72sZu3qdO.jpg"},
            //    {"KinoPop", "/t/p/original/gzHzhgt6cVSn4yy6UnJvLGbOSwL.jpg"},
            //    {"Oi Play", "/t/p/original/xbdgLcQ6kRrcVe1uJAG9lzlkSbY.jpg"},
            //    {"MGM Amazon Channel", "/t/p/original/fUUgfrOfvvPKx9vhFBd6IMdkfLy.jpg"},
            //    {"Microsoft Store", "/t/p/original/shq88b09gTBYC4hA7K7MUL8Q4zP.jpg"},
            //    {"Looke Amazon Channel", "/t/p/original/3gTVbIj15Amgz5Qqg5dPDpgMW9V.jpg"},
            //    {"Revry", "/t/p/original/r1UgUKmt83FSDOIHBdRWKooZPNx.jpg"},
            //    {"MovieSaints", "/t/p/original/fdWE8jpmQqkZrwg2ZMuCLz6ms5P.jpg"},
            //    {"Noggin Amazon Channel", "/t/p/original/yxBUPUBFzHE72uFXvFr1l0fnMJA.jpg"},
            //    {"History Play", "/t/p/original/73ms51HSpkD0OOXwj2EeiZeSqSt.jpg"}

            //};
            #endregion

            #region ExcelCreate
            //CreateExcelFileIfIsNotExists();
            //using (var package = new ExcelPackage(_filePath))
            //{
            //    var mainSheet = package.Workbook.Worksheets[0];
            //    if (mainSheet.Dimension != null)
            //    {
            //        rowIndex = mainSheet.Dimension.Rows;
            //    }
            //    else
            //    {
            //        rowIndex = 1;
            //    }
            //}
            #endregion
        }

        private async Task<bool> IsContainsInExcel(string name, string provider)
        {
            using (var package = new ExcelPackage(_filePath))
            {
                //package.Workbook.Worksheets.Add("dd");
                ExcelWorksheet mainSheet;
                var isContainsMainSheet = package.Workbook.Worksheets.Any(el => el.Name == provider);
                if (!isContainsMainSheet)
                {
                    //Console.WriteLine("none");
                    mainSheet = package.Workbook.Worksheets.Add(provider);
                    await package.SaveAsync();
                    return false;
                }
                mainSheet = package.Workbook.Worksheets.First(el => el.Name == provider);
                if (mainSheet.Dimension != null)
                {
                    for (int i = 1; i <= mainSheet.Dimension.Rows; i++)
                    {
                        if (mainSheet.Cells[i, 2].Value as string == name)
                        {
                            //Console.WriteLine("Break;");
                            return true;
                        }
                    }
                }

                return false;

                //mainSheet.Cells[rowIndex, 3].Value = GetFinalRedirect(link);
                //mainSheet.Cells[rowIndex, 1].Value = title;
                //mainSheet.Cells[rowIndex, 2].Value = item;
                //await _tmpPage.CloseAsync();
            }
        }

        public async Task Start()
        {
            await BrowserLoader($"https://www.themoviedb.org/{_configuration.ParserStartCategory}", 5);//_category
        }
        public async Task InitBrowserAsync()
        {
            _browser = await Puppeteer.LaunchAsync(_options);
        }
        public async Task<Page> GetPageAsync()
        {
            return await _browser.NewPageAsync();
        }

        //private void SetExcelStyles()
        //{
        //    using (var package = new ExcelPackage(_filePath))
        //    {
        //        package.Workbook.
        //        WorkbookStylesPart stylesPart = excel.WorkbookPart.AddNewPart<WorkbookStylesPart>();
        //    stylesPart.Stylesheet = GenerateStyleSheet();
        //    stylesPart.Stylesheet.Save();
        //        }
        //}

        private async Task BrowserLoader(string url, int count)
        {
            try
            {
                await InitBrowserAsync();
                _page = await GetPageAsync();
                string login = File.ReadAllLines("settings.txt")[3];
                string password = File.ReadAllLines("settings.txt")[4];
                await _page.AuthenticateAsync(new Credentials() { Username = login, Password = password });
                //foreach (var apage in _browser.PagesAsync)
                //{

                //}
                //await (await _browser.PagesAsync())[0].CloseAsync();
                var responce = await _page.GoToAsync(url);
                await _page.ClickAsync("div.filter_panel:nth-of-type(3)");
                await _page.WaitForSelectorAsync("div.filter_panel:nth-of-type(3)");
                await _page.ClickAsync("div.filter_panel:nth-of-type(3) span.k-dropdown-wrap");
                await _page.WaitForSelectorAsync("span.k-list-filter");
                await _page.FocusAsync("input.k-textbox");
                await _page.Keyboard.TypeAsync("Бразилия");
                Thread.Sleep(1000);
                await _page.Keyboard.PressAsync("Enter");
                var providersImages = await _page.EvaluateExpressionAsync<dynamic>("Array.from(document.querySelectorAll('ul#ott_providers > li:not(.hidden) a > img')).map(img => [img.src, img.alt]);");
                foreach (var provider in providersImages)
                {
                    //Console.WriteLine((string)provider[1]);
                    //Console.WriteLine((string)provider[0]);
                    if (!_providers.ContainsKey((string)provider[1]))
                    {
                        _providers.Add((string)provider[1], (string)provider[0]);
                        await ApiClient.CreatePlatformIfIsNotExist((string)provider[1], (string)provider[0]);
                    }
                }

                if (_configuration.Platforms != null)
                {
                    foreach (var platformId in _configuration.Platforms)
                    {
                        try
                        {
                            var platformSrc = (await ApiClient.GetPlatformById(platformId)).ImageUrl.Substring(26);
                            if (platformSrc != null && await _page.EvaluateExpressionAsync<bool>($"document.querySelectorAll('img[src|=\"{platformSrc}\"]').length > 0"))
                            {
                                await _page.ClickAsync($"img[src|=\"{platformSrc}\"");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                await _page.EvaluateExpressionAsync("let arr = [];");
                //Console.WriteLine(providersImages[0]);
                //Thread.Sleep(3000);
                //await _page.ClickAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]");
                await _page.Mouse.ClickAsync(388, 568, null);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.UtcNow + " " + ex.Message);
                if (ex.Message.Contains("Timeout of") && count < 5)
                {
                    await _browser.CloseAsync();
                    count++;
                    await BrowserLoader(url, count);
                }
            }
            while (true)
            {
                await DataActions();
            }


            try
            {
                await _browser.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetProviderName(string url)
        {
            foreach (var item in _providers)
            {

                if (url.Contains(item.Value))
                {
                    if (item.Key.Contains("Amazon")) // || item.Key.Contains("Paramount Plus")
                    {
                        return "Amazon Channel";
                    }
                    return item.Key;
                }
            }
            return null;
        }

        private async Task<bool> ExecuteData(string item)
        {
            //if (Console.ReadLine() == "23")
            //{
            //    break;
            //}
            try
            {
                _tmpPage = await _browser.NewPageAsync();
                await _tmpPage.GoToAsync(item);
                var query = $"document.querySelector('div.title > h2 > a').innerText";
                string season = "", episode = "";
                //Console.ReadKey();
                var title = await _tmpPage.EvaluateExpressionAsync<string>(query);
                //Console.ReadKey();
                if (_configuration.ParserStartCategory == "tv")//_category == "tv"
                {
                    season = await _tmpPage.EvaluateExpressionAsync<string>(
                        "document.querySelector('div.season > div.flex > div.content h2').innerText");
                    episode = await _tmpPage.EvaluateExpressionAsync<string>(
                        "document.querySelector('div.season > div.flex > div.content h4').innerText");
                }
                List<CustomProvider> providers = new List<CustomProvider>();
                //await _tmpPage.WaitForSelectorAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]",
                //await _tmpPage.WaitForSelectorAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]",
                // new WaitForSelectorOptions { Visible = true });
                //await _tmpPage.ClickAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]");
                if (await _tmpPage.QuerySelectorAsync("div.provider") != null)
                {
                    await _tmpPage.ClickAsync("div.provider");
                    //await _tmpPage.WaitForNavigationAsync();
                    await _tmpPage.WaitForSelectorAsync("li.ott_filter_best_price > div > a");
                    var links = await _tmpPage.EvaluateExpressionAsync<string[]>(
                        "Array.from(document.querySelector('ul.providers').querySelectorAll('li:not(.hide) a')).map(a => a.href)");
                    var imagesSrc = await _tmpPage.EvaluateExpressionAsync<string[]>(
                        "Array.from(document.querySelector('ul.providers').querySelectorAll('li:not(.hide) a > img')).map(img => img.src)");
                    if (imagesSrc.Length > 0)
                    {
                        //providersNames.AddRange(imagesSrc.Select(GetProviderName));
                        foreach (var imgSrc in imagesSrc)
                        {
                            providers.Add(new CustomProvider { Name = GetProviderName(imgSrc) });
                        }
                    }
                }
                else
                {
                    providers.Add(new CustomProvider { Name = "Without platform" });
                }
                SerialDTO serial = null;
                MovieDTO movie = null;
                //List<string> providersNames = new List<string>();


                //else
                //{

                //}
                if (_configuration.ParserStartCategory == "tv")//_category == "tv"
                {
                    serial = await ApiClient.GetSerialByUrl(item);
                    if (serial == null)
                    {
                        serial = new SerialDTO();
                        serial.Name = title;
                        serial.Url = item;
                        serial.Seasons = season;
                        serial.Series = episode;
                        await ApiClient.AddSerial(serial);
                        serial = await ApiClient.GetSerialByUrl(serial.Url);
                    }
                    else
                    {
                        if ((serial.Seasons != season || serial.Series != episode))//serial.Seasons != null && serial.Series != null && 
                        {
                            serial.IsUpdated = true;
                            serial.Seasons = season;
                            serial.Series = episode;
                        }
                        serial.Name = title;
                        serial.Url = item;
                        await ApiClient.UpdateSerial(serial);
                    }
                    await ApiClient.SetSerialPlatforms(providers, serial.Id);


                }
                else
                {
                    movie = await ApiClient.GetFilmByUrl(item);
                    if (movie == null)
                    {
                        movie = new MovieDTO();
                        movie.Name = title;
                        movie.Url = item;
                        await ApiClient.AddMovie(movie);
                        movie = await ApiClient.GetFilmByUrl(movie.Url);
                    }
                    else
                    {
                        movie.Name = title;
                        await ApiClient.UpdateMovie(movie);
                    }
                    await ApiClient.SetMoviePlatforms(providers, movie.Id);

                }

                //if (serial != null || movie != null)
                //if (await IsContainsInDatabase(item, "Without platform"))
                //{
                //if (_category == "tv")
                //{

                //}
                //else
                //{
                //    //movie. 
                //}
                //using (var package = new ExcelPackage(_filePath))
                //{
                //    var mainSheet =
                //        package.Workbook.Worksheets.First(el => el.Name == "Without platform");
                //    int rowIndexPos = 1;
                //    for (int j = 1; j <= mainSheet.Dimension.Rows; j++)
                //    {
                //        if (mainSheet.Cells[j, 2].Value as string == item)
                //        {
                //            rowIndexPos = j;
                //            break;
                //        }
                //    }

                //    mainSheet.Cells[rowIndexPos, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    mainSheet.Cells[rowIndexPos, 1].Style.Fill.BackgroundColor
                //        .SetColor(ColorTranslator.FromHtml("#03f215"));
                //}
                //}

                //if (links.Length > 1)
                //{
                //Console.WriteLine("this");
                //var imagesSrc = await _tmpPage.EvaluateExpressionAsync<string[]>(
                //    "Array.from(document.querySelector('ul.providers').querySelectorAll('li:not(.hide) a > img')).map(img => img.src)");



                //Console.ReadKey();
                //}
                //else
                //{
                //    var link = await _tmpPage.EvaluateExpressionAsync<string>(
                //        "document.querySelector('li.ott_filter_best_price > div > a').href");
                //    var providerLink =
                //        await _tmpPage.EvaluateExpressionAsync<string>(
                //            "document.querySelector('li.ott_filter_best_price > div > a > img').src");
                //    var provider = GetProviderName(providerLink);

                //    if (!(await IsContainsInExcel(item, provider)))
                //    {
                //        using (var package = new ExcelPackage(_filePath))
                //        {
                //            //var mainSheet = package.Workbook.Worksheets.First(el => el.Name == provider);
                //            //int rowIndexPos = 1;
                //            //if (mainSheet.Dimension != null)
                //            //{
                //            //    rowIndexPos = mainSheet.Dimension.Rows + 1;
                //            //}

                //            ////Console.WriteLine(rowIndexPos);
                //            //mainSheet.Cells[rowIndexPos, 3].Value = GetFinalRedirect(link);
                //            //mainSheet.Cells[rowIndexPos, 1].Value = title;
                //            //mainSheet.Cells[rowIndexPos, 2].Value = item;
                //            if (_category == "tv")
                //            {
                //                //mainSheet.Cells[rowIndexPos, 4].Value = season;
                //                //mainSheet.Cells[rowIndexPos, 5].Value = episode;
                //                await _tmpPage.ClickAsync("section.panel.season p.new_button");
                //                //await _tmpPage.WaitForNavigationAsync();
                //                await _tmpPage.WaitForSelectorAsync("div.season_wrapper h4");
                //                var sum = await _tmpPage.EvaluateExpressionAsync<int>(
                //                    @"Array.from(document.querySelectorAll('div.season_wrapper h4')).map(h4 => +(h4.innerText.substring(h4.innerText.indexOf('|')+2)).match(/\d+/g)).reduce((partialSum, a) => partialSum + a, 0)");
                //                //mainSheet.Cells[rowIndexPos, 6].Value = sum;
                //            }

                //            ++rowIndex;
                //            await package.SaveAsync();
                //        }
                //    }
                //    else
                //    {
                //        using (var package = new ExcelPackage(_filePath))
                //        {
                //            var mainSheet = package.Workbook.Worksheets.First(el => el.Name == provider);
                //            int rowIndexPos = 1;
                //            for (int j = 1; j <= mainSheet.Dimension.Rows; j++)
                //            {
                //                if (mainSheet.Cells[j, 2].Value as string == item)
                //                {
                //                    rowIndexPos = j;
                //                    break;
                //                }
                //            }

                //            if (_category == "tv")
                //            {
                //                //if (mainSheet.Cells[rowIndexPos, 4].Value as string != season)
                //                //{
                //                //    mainSheet.Cells[rowIndexPos, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //                //    mainSheet.Cells[rowIndexPos, 4].Style.Fill.BackgroundColor
                //                //        .SetColor(ColorTranslator.FromHtml("#03f215"));
                //                //    mainSheet.Cells[rowIndexPos, 4].Value = season;
                //                //}

                //                //if (mainSheet.Cells[rowIndexPos, 5].Value as string != episode)
                //                //{
                //                //    mainSheet.Cells[rowIndexPos, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //                //    mainSheet.Cells[rowIndexPos, 5].Style.Fill.BackgroundColor
                //                //        .SetColor(ColorTranslator.FromHtml("#03f215"));
                //                //    mainSheet.Cells[rowIndexPos, 5].Value = episode;
                //                //}

                //                await _tmpPage.ClickAsync("section.panel.season p.new_button");
                //                await _tmpPage.WaitForSelectorAsync("div.season_wrapper h4");
                //                var sum = await _tmpPage.EvaluateExpressionAsync<int>(
                //                    @"Array.from(document.querySelectorAll('div.season_wrapper h4')).map(h4 => +(h4.innerText.substring(h4.innerText.indexOf('|')+2)).match(/\d+/g)).reduce((partialSum, a) => partialSum + a, 0)");
                //                //if (mainSheet.Cells[rowIndexPos, 6].Value as int? != sum)
                //                //{
                //                //    mainSheet.Cells[rowIndexPos, 6].Value = sum;
                //                //}

                //                //++rowIndex;
                //                //await package.SaveAsync();
                //            }
                //        }
                //    }
                //}


                //else
                //{
                //    if (!await IsSerialContainsInDatabase(item))
                //    {
                //        //using (var package = new ExcelPackage(_filePath))
                //        //{
                //        //    var mainSheet = package.Workbook.Worksheets.First(el => el.Name == "Without platform");
                //        //    int rowIndexPos = 1;
                //        //    if (mainSheet.Dimension != null)
                //        //    {
                //        //        rowIndexPos = mainSheet.Dimension.Rows + 1;
                //        //    }

                //        //    mainSheet.Cells[rowIndexPos, 1].Value = title;
                //        //    mainSheet.Cells[rowIndexPos, 2].Value = item;
                //            if (_category == "tv")
                //            {
                //                //mainSheet.Cells[rowIndexPos, 3].Value = season;
                //                //mainSheet.Cells[rowIndexPos, 4].Value = episode;
                //                await _tmpPage.ClickAsync("section.panel.season p.new_button");
                //                //await _tmpPage.WaitForNavigationAsync();
                //                await _tmpPage.WaitForSelectorAsync("div.season_wrapper h4");
                //                var sum = await _tmpPage.EvaluateExpressionAsync<int>(
                //                    @"Array.from(document.querySelectorAll('div.season_wrapper h4')).map(h4 => +(h4.innerText.substring(h4.innerText.indexOf('|')+2)).match(/\d+/g)).reduce((partialSum, a) => partialSum + a, 0)");
                //                //mainSheet.Cells[rowIndexPos, 5].Value = sum;
                //            }

                //            //await package.SaveAsync();
                //        //KC}
                //    }
                //    else
                //    {
                //        //using (var package = new ExcelPackage(_filePath))
                //        //{
                //        //    var mainSheet = package.Workbook.Worksheets.First(el => el.Name == "Without platform");
                //        //    int rowIndexPos = 1;
                //        //    for (int j = 1; j <= mainSheet.Dimension.Rows; j++)
                //        //    {
                //        //        if (mainSheet.Cells[j, 2].Value as string == item)
                //        //        {
                //        //            rowIndexPos = j;
                //        //            break;
                //        //        }
                //        //    }

                //            if (_category == "tv")
                //            {
                //                //if (mainSheet.Cells[rowIndexPos, 3].Value as string != season)
                //                //{
                //                //    mainSheet.Cells[rowIndexPos, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //                //    mainSheet.Cells[rowIndexPos, 3].Style.Fill.BackgroundColor
                //                //        .SetColor(ColorTranslator.FromHtml("#03f215"));
                //                //    mainSheet.Cells[rowIndexPos, 3].Value = season;
                //                //}

                //                //if (mainSheet.Cells[rowIndexPos, 4].Value as string != episode)
                //                //{
                //                //    mainSheet.Cells[rowIndexPos, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //                //    mainSheet.Cells[rowIndexPos, 4].Style.Fill.BackgroundColor
                //                //        .SetColor(ColorTranslator.FromHtml("#03f215"));
                //                //    mainSheet.Cells[rowIndexPos, 4].Value = episode;
                //                //}

                //                await _tmpPage.ClickAsync("section.panel.season p.new_button");
                //                await _tmpPage.WaitForSelectorAsync("div.season_wrapper h4");
                //                var sum = await _tmpPage.EvaluateExpressionAsync<int>(
                //                    @"Array.from(document.querySelectorAll('div.season_wrapper h4')).map(h4 => +(h4.innerText.substring(h4.innerText.indexOf('|')+2)).match(/\d+/g)).reduce((partialSum, a) => partialSum + a, 0)");
                //                //if (mainSheet.Cells[rowIndexPos, 5].Value as int? != sum)
                //                //{
                //                //    mainSheet.Cells[rowIndexPos, 5].Value = sum;
                //                //}
                //            }

                //            //++rowIndex;
                //            //await package.SaveAsync();
                //        }

                //}

                //if (await _tmpPage.QuerySelectorAsync("ul.providers") != null)
                //{

                //}
                await _tmpPage.CloseAsync();
            }
            //;

            catch (Exception ex)
            {
                Console.WriteLine(DateTime.UtcNow + " " + ex.Message + " on " + item);
                Console.WriteLine(ex.StackTrace);
                await _tmpPage.CloseAsync();
                return false;
            }
            return true;
        }

        private async Task<bool> IsFilmContainsInDatabase(string item)
        {
            return (await ApiClient.GetFilmByUrl(item)) != null;
        }
        private async Task<bool> IsSerialContainsInDatabase(string item)
        {
            return (await ApiClient.GetSerialByUrl(item)) != null;
        }

        private async Task<bool> DataActions()
        {
            try
            {

                //await _page.WaitForSelectorAsync("#page_" + pageIndex);
                var jsSelectAllAnchors =
                @"arr = document.querySelectorAll('div.page_wrapper'); Array.from(document.querySelectorAll(`#${arr[arr.length - 1].id} > div > div > div > a:not(.no_click)`)).map(a => a.href);";
                //var jsSelectAllAnchors =
                //@$"Array.from(document.querySelectorAll('#page_{pageIndex} > div > div > div > a')).map(a => a.href);";
                var urls = await _page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);
                //Console.WriteLine(String.Join(' ', urls));
                //Console.WriteLine(pageIndex);
                urls = urls.Where(url =>
                    !url.Contains($"https://www.themoviedb.org/{_configuration.ParserStartCategory}?page=") && //_category
                    !url.Contains($"https://www.themoviedb.org/{_configuration.ParserStartCategory}#")).ToArray(); //_category
                if (!urls.Any())
                {
                    Console.WriteLine("Page haven`t urls");
                }

                //if (pageIndex > 50)

                // Console.WriteLine(_configuration.Count);

                foreach (var item in urls)
                {
                    if (_configuration.Count.HasValue && counter >= _configuration.Count)
                    {
                        await _browser.CloseAsync();
                        Environment.Exit(0);
                    }
                    await ExecuteData(item);
                    ++counter;
                }
                //}


                await _page.EvaluateExpressionAsync("window.scrollBy(0, document.body.scrollHeight)");
                Thread.Sleep(1000);
                if (pageIndex == 1)
                {
                    await _page.ClickAsync("div.pagination > p.load_more > a");
                }
                Thread.Sleep(2000);
                //await _page.Wait();
                ++pageIndex;
            }
            catch (Exception ex)
            {
                await _page.EvaluateExpressionAsync("window.scrollBy(0, document.body.scrollHeight)");
                Console.WriteLine(DateTime.UtcNow + " " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                await _tmpPage?.CloseAsync()!;
                return false;
            }
            return true;

        }

        public static string GetFinalRedirect(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            int maxRedirCount = 8;  // prevent infinite loops
            string newUrl = url;
            do
            {
                HttpWebRequest req = null;
                HttpWebResponse resp = null;
                try
                {
                    req = (HttpWebRequest)HttpWebRequest.Create(url);
                    req.Method = "HEAD";
                    req.AllowAutoRedirect = false;
                    resp = (HttpWebResponse)req.GetResponse();
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            return newUrl;
                        case HttpStatusCode.Redirect:
                        case HttpStatusCode.MovedPermanently:
                        case HttpStatusCode.RedirectKeepVerb:
                        case HttpStatusCode.RedirectMethod:
                            newUrl = resp.Headers["Location"];
                            if (newUrl == null)
                                return url;

                            if (newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1)
                            {
                                // Doesn't have a URL Schema, meaning it's a relative or absolute URL
                                Uri u = new Uri(new Uri(url), newUrl);
                                newUrl = u.ToString();
                            }
                            break;
                        default:
                            return newUrl;
                    }
                    url = newUrl;
                }
                catch (WebException)
                {
                    // Return the last known good URL
                    return newUrl;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (resp != null)
                        resp.Close();
                }
            } while (maxRedirCount-- > 0);

            return newUrl;
        }
    }
}
