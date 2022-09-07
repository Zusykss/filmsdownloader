using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MoviesParser
{
    public static class ApiClient
    {
        static HttpClient client = new HttpClient();
        private static readonly string _apiPath = File.ReadAllLines("settings.txt")[6];//;//""
        public static async Task CreatePlatformIfIsNotExist(string platformName)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(_apiPath+ "Platform/addIfNotExists", new {Name = platformName});
        }
    }
}
