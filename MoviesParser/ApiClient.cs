using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MoviesParser.DTO_s;
using MoviesParser.Enums;

namespace MoviesParser
{
    public static class ApiClient
    {
        static HttpClient client = new HttpClient();
        private static readonly string _apiPath = File.ReadAllLines("settings.txt")[6];//;//""
        public static async Task CreatePlatformIfIsNotExist(string platformName, string imageUrl)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(_apiPath+ "Platform/addIfNotExists", new {Name = platformName, ImageUrl = imageUrl});
        }

        public static async Task<MovieDTO> GetFilmByUrl(string url)
        {
            return await client.GetFromJsonAsync<MovieDTO>(_apiPath + $"Movie/getMovieByUrl?url={url}");
        }
        public static async Task<SerialDTO> GetSerialByUrl(string url)
        {
            return await client.GetFromJsonAsync<SerialDTO>(_apiPath + $"Serial/getSerialByUrl?url={url}");
        }

        public static async Task AddSerial(SerialDTO serial)
        {
            await client.PostAsJsonAsync(_apiPath + "Serial/addSerial", serial);
        }
        public static async Task AddMovie(MovieDTO movie)
        {
            await client.PostAsJsonAsync(_apiPath + "Movie/addMovie", movie);
        }

        public static async Task UpdateSerial(SerialDTO serial)
        {
            await client.PostAsJsonAsync(_apiPath + "Serial/editSerial", serial);
        }
        public static async Task UpdateMovie(MovieDTO movie)
        {
            await client.PostAsJsonAsync(_apiPath + "Movie/editMovie", movie);
        }
    }
}
