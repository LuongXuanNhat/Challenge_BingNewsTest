using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces.IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity entity);
        Task<TEntity> GetById(string id);
        Task Update(TEntity entity);
        Task Delete(string id);
        Task<IEnumerable<TEntity>> GetAll();
    }
}
