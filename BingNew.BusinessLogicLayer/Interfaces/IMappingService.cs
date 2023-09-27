namespace BingNew.BusinessLogicLayer.Interfaces
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource source);
        Task<TDestination> MapAsync<TSource, TDestination>(TSource source);


    }
}
