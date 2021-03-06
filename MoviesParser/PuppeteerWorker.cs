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
        private FileInfo _filePath;
        private string _proxy = File.ReadAllLines("settings.txt")[2];

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

        public PuppeteerWorker(string category = "movie")
        {
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
                  },
                IgnoredDefaultArgs = new string[]
                {
                    "--enable-automation"
                },
                ExecutablePath = File.ReadAllLines("settings.txt")[0]

            };
            _category = category;
            //_filePath = new FileInfo(File.ReadAllLines("settings.txt")[1]);
            // Ставимо ліцензію (не комерційну) на OfficeOpenXml
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            CreateExcelFileIfIsNotExists();
            using (var package = new ExcelPackage(_filePath))
            {
                var mainSheet = package.Workbook.Worksheets[0];
                if (mainSheet.Dimension != null)
                {
                    rowIndex = mainSheet.Dimension.Rows;
                }
                else
                {
                    rowIndex = 1;
                }
            }
        }

        private bool IsContainsInExcel(string name)
        {
            using (var package = new ExcelPackage(_filePath))
            {
                var mainSheet = package.Workbook.Worksheets[0];
                if (mainSheet.Dimension != null)
                {
                    for (int i = 1; i < mainSheet.Dimension.Rows; i++)
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
                //await package.SaveAsync();
            }
        }

        public async Task Start()
        {
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
                Thread.Sleep(1000);
                await _page.ClickAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]");
                await _page.Mouse.ClickAsync(388, 568, null);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

        private async Task<bool> DataActions()
        {
            try
            {
                var jsSelectAllAnchors =
                    @$"Array.from(document.querySelectorAll('#page_{pageIndex} > div > div > div > a')).map(a => a.href);";
                var urls = await _page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);
                urls = urls.Where(url =>
                    !url.Contains($"https://www.themoviedb.org/{_category}?page=") &&
                    !url.Contains($"https://www.themoviedb.org/{_category}#")).ToArray();
                foreach (var item in urls)
                {
                    _tmpPage = await _browser.NewPageAsync();
                    await _tmpPage.GoToAsync(item);
                    var query = $"document.querySelector('div.title > h2 > a').innerText";
                    var title = await _tmpPage.EvaluateExpressionAsync<string>(query);
                    await _tmpPage.WaitForSelectorAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]",
                        new WaitForSelectorOptions { Visible = true });
                    await _tmpPage.ClickAsync("img[src|=\"/t/p/original/t2yyOv40HZeVlLjYsCsPHnWLk4W.jpg\"]");
                    await _tmpPage.WaitForNavigationAsync();
                    var link = await _tmpPage.EvaluateExpressionAsync<string>(
                        "document.querySelector('li.ott_filter_best_price > div > a').href");
                    if (!IsContainsInExcel(item))
                    {
                        using (var package = new ExcelPackage(_filePath))
                        {
                            var mainSheet = package.Workbook.Worksheets[0];
                            mainSheet.Cells[rowIndex, 3].Value = GetFinalRedirect(link);
                            mainSheet.Cells[rowIndex, 1].Value = title;
                            mainSheet.Cells[rowIndex, 2].Value = item;
                            ++rowIndex;
                            await package.SaveAsync();
                        }
                    }
                            await _tmpPage.CloseAsync();

                    
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
