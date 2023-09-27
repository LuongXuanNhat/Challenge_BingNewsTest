namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<bool> Add(TEntity entity);
        Task<TEntity> GetById(string id);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(string id);
        Task<IEnumerable<TEntity>> GetAll();
    }
}
