using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource source);
        Task<TDestination> MapAsync<TSource, TDestination>(TSource source);


    }
}
