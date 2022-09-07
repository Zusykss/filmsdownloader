using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Classes;

namespace Core.Helpers.Converters
{
    public class PagedListConverter//<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>> where TSource : class where TDestination : class
    {
        private readonly IMapper _mapper;

        public PagedListConverter(IMapper mapper)
        {
            _mapper = mapper;
        }
        //public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        //{
        //    var collection = _mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);

        //    return new PagedList<TDestination>(collection, source.Page, source.Size, source.TotalCount);
        //}
    }
}
