using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity entity);
        Task<TEntity> GetById(string id);
        void Update(TEntity entity);
        void Delete(string id);
        IEnumerable<TEntity> GetAll();
    }
}
