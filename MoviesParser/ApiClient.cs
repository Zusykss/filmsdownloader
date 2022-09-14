﻿using System;
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
            HttpResponseMessage response = await client.PostAsJsonAsync(_apiPath+ "Platform/addIfNotExists", new {Name = platformName});
        }

        public static async Task<MovieDTO> GetFilmByUrl(string url)
        {
            return await client.GetFromJsonAsync<MovieDTO>(_apiPath + $"Movie/addIfNotExists?url={url}");
        }
        public static async Task<SerialDTO> GetSerialByUrl(string url)
        {
            return await client.GetFromJsonAsync<SerialDTO>(_apiPath + $"Serial/addIfNotExists?url={url}");
        }
    }
}
