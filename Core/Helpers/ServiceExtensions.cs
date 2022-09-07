using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.CustomServices;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Helpers
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPlatformService, PlatformService>();
            services.AddScoped<IMovieService, MovieService>();
            //services.AddScoped<IAuthorService, AuthorService>();
            //services.AddScoped<ITableService, TableService>();
            //services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IJwtService, JwtService>();
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var configures = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Platform, PlatformDTO>().ReverseMap();
                mc.CreateMap<Movie, MovieDTO>().ReverseMap();
                mc.CreateMap<Serial, SerialDTO>().ReverseMap();
                //mc.CreateMap<Table, TableDTO>().ReverseMap();
            });

            IMapper mapper = configures.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
