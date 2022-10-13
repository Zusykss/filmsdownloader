using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using MoviesParser.DTO_s;
using MoviesParser.Enums;
using Newtonsoft.Json;

namespace MoviesParser
{
    public static class ApiClient
    {
        private static HttpClient client;
        private static readonly string _apiPath = File.ReadAllLines("settings.txt")[6];//;//""
        private static HttpClientHandler _clientHandler = new HttpClientHandler();

        static ApiClient()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(_clientHandler);
        }
        public static async Task CreatePlatformIfIsNotExist(string platformName, string imageUrl)
        {
            //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            HttpResponseMessage response = await client.PostAsJsonAsync(_apiPath+ "Platform/addIfNotExists", new {Name = platformName, ImageUrl = imageUrl});
        }

        public static async Task<MovieDTO> GetFilmByUrl(string url)
        {
            var movieJson = await client.GetStringAsync(_apiPath + $"Movie/getMovieByUrl?url={url}");
            var result = String.IsNullOrEmpty(movieJson) ? null : JsonConvert.DeserializeObject<MovieDTO>(movieJson);
            return result;
            //return await client.GetFromJsonAsync<MovieDTO>(_apiPath + $"Movie/getMovieByUrl?url={url}");
        }
        public static async Task<SerialDTO> GetSerialByUrl(string url)
        {
            var serialJson = await client.GetStringAsync(_apiPath + $"Serial/getSerialByUrl?url={url}");
            var result = String.IsNullOrEmpty(serialJson) ? null : JsonConvert.DeserializeObject<SerialDTO>(serialJson);
            return result;
            //return await client.GetFromJsonAsync<SerialDTO>(_apiPath + $"Serial/getSerialByUrl?url={url}");
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

        public static async Task SetSerialPlatforms(List<CustomProvider> providers, int id)
        {
            await client.PostAsJsonAsync(_apiPath + "Serial/setPlatformsByNames?id=" + id, providers);
        }public static async Task SetMoviePlatforms(List<CustomProvider> providers, int id)
        {
            await client.PostAsJsonAsync(_apiPath + "Movie/setPlatformsByNames?id=" + id, providers);
        }

        public static async Task<PlatformDTO> GetPlatformById(int platformId)
        {
            return await client.GetFromJsonAsync<PlatformDTO>(_apiPath + $"Platform/getById?id={platformId}");
        }
    }
}
