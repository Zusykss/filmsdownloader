using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.Response;
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
            services.AddScoped<ISerialService, SerialService>();
            services.AddScoped<IParserService, ParserService>();
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
                mc.CreateMap<Movie, MovieResponseDTO>().ForMember(dest => dest.Platforms, act => act.MapFrom(src => src.PlatformsMovies.Select(p => p.Movie.Name))).ForMember(dest => dest.Status, act => act.MapFrom(src => src.Status.Name)).ReverseMap();
                mc.CreateMap<Serial, SerialResponseDTO>().ForMember(dest => dest.Platforms, act => act.MapFrom(src => src.PlatformsSerials.Select(p => p.Serial.Name))).ForMember(dest => dest.Status, act => act.MapFrom(src => src.Status.Name)).ReverseMap();
                mc.CreateMap<PlatformMovie, PlatformMovieDTO>().ReverseMap();
                mc.CreateMap<PlatformSerial, PlatformSerialDTO>().ReverseMap();
                //mc.CreateMap<Table, TableDTO>().ReverseMap();
            });

            IMapper mapper = configures.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
