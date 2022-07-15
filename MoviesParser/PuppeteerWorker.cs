using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using Page = PuppeteerSharp.Page;

namespace MoviesParser
{
    public class PuppeteerWorker
    {
        private Browser _browser;
        private Page _page;
        private Page _tmpPage;
        private LaunchOptions _options;
        private string _category;
        int rowIndex = 1;
        int pageIndex = 1;
        private string _proxy = File.ReadAllLines("settings.txt")[2];
        public PuppeteerWorker(string category = "movie")
        {
            _options = new LaunchOptions
            {
                Headless = false,
                Args = new[]{"--disable-gpu",
                    "--disable-dev-shm-usage",
                    "--disable-setuid-sandbox",
                    "--no-first-run",
                    "--no-sandbox",
                    "--no-zygote",
                    $"--proxy-server=http://{_proxy}",
                    "--deterministic-fetch",
                    "--disable-features=IsolateOrigins",
                    "--disable-site-isolation-trials",
                    "--start-maximized"},
                IgnoredDefaultArgs = new string[]
                {
                    "--enable-automation"
                },
                ExecutablePath = File.ReadAllLines("settings.txt")[0]

            };
            _category = category;
        }
        public async Task Start()
        {
            await InitBrowserAsync();
            await BrowserLoader($"https://www.themoviedb.org/{_category}", 5);
        }
        public async Task InitBrowserAsync()
        {
            _browser = await Puppeteer.LaunchAsync(_options);
        }
        public async Task<Page> GetPageAsync()
        {
            return await _browser.NewPageAsync();
        }

        private async Task<string> BrowserLoader(string url, int count)
        {
            string text = "";
            try
            {
                await InitBrowserAsync();
                _page = await GetPageAsync();
                await _page.AuthenticateAsync(new Credentials() { Username = "vxjeAx", Password = "3jJLSH" });
                var responce = await _page.GoToAsync(url);
                text = await responce.TextAsync();
                await _page.ClickAsync("div.filter_panel:nth-of-type(3)");
                await _page.WaitForSelectorAsync("div.filter_panel:nth-of-type(3)");
                await _page.ClickAsync("div.filter_panel:nth-of-type(3) span.k-dropdown-wrap");
                await _page.WaitForSelectorAsync("span.k-list-filter");
                await _page.FocusAsync("input.k-textbox");
                await _page.Keyboard.TypeAsync("Бразилия");
                Thread.Sleep(1000);
                await _page.Keyboard.PressAsync("Enter");
                Thread.Sleep(1000);
                await _page.ClickAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]");
                await _page.Mouse.ClickAsync(388, 568, null);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                text = "ERROR BrowserLoader " + ex.Message;
                if (ex.Message.Contains("Timeout of") && count < 5)
                {
                    await _browser.CloseAsync();
                    count++;
                    return await BrowserLoader(url, count);
                }
            }

            // Ставимо ліцензію (не комерційну) на OfficeOpenXml
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var filePath = new FileInfo((await File.ReadAllLinesAsync("settings.txt"))[1]);
            using (var package = new ExcelPackage(filePath))
            {
                var mainSheet = package.Workbook.Worksheets[0];
                
                while (true)
                {
                    await DataActions(mainSheet, package);
                }
            }

            try
            {
                await _browser.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return text;
        }

        private async Task<bool> DataActions(ExcelWorksheet mainSheet, ExcelPackage package)
        {
            try
            {
                var jsSelectAllAnchors =
                    @$"Array.from(document.querySelectorAll('#page_{pageIndex} > div > div > div > a')).map(a => a.href);";
                var urls = await _page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);
                urls = urls.Where(url =>
                    !url.Contains("https://www.themoviedb.org/movie?page=") &&
                    !url.Contains("https://www.themoviedb.org/movie#")).ToArray();
                foreach (var item in urls)
                {
                    _tmpPage = await _browser.NewPageAsync();
                    await _tmpPage.GoToAsync(item);
                    var query = $"document.querySelector('div.title > h2 > a').innerText";
                    var title = await _tmpPage.EvaluateExpressionAsync<string>(query);
                    await _tmpPage.WaitForSelectorAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]",
                        new WaitForSelectorOptions {Visible = true});
                    await _tmpPage.ClickAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]");
                    await _tmpPage.WaitForNavigationAsync();
                    var link = await _tmpPage.EvaluateExpressionAsync<string>(
                        "document.querySelector('li.ott_filter_best_price > div > a').href");
                    mainSheet.Cells[rowIndex, 3].Value = GetFinalRedirect(link);
                    mainSheet.Cells[rowIndex, 1].Value = title;
                    mainSheet.Cells[rowIndex, 2].Value = item;
                    ++rowIndex;
                    await _tmpPage.CloseAsync();
                    await package.SaveAsync();
                }

                await _page.EvaluateExpressionAsync("window.scrollBy(0, document.body.scrollHeight)");
                Thread.Sleep(1000);
                if (pageIndex == 1)
                {
                    await _page.ClickAsync("div.pagination > p.load_more > a");
                    Thread.Sleep(1000);
                }
                ++pageIndex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
