using BingNew.BusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Services
{
    public class MappingService : IMappingService
    {
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var destination = Activator.CreateInstance<TDestination>();

            foreach (var sourceProperty in typeof(TSource).GetProperties())
            {
                var destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name);

                if (destinationProperty != null)
                {
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }

            return destination;
        }

        public Task<TDestination> MapAsync<TSource, TDestination>(TSource source)
        {
            throw new NotImplementedException();
        }
    }
}
