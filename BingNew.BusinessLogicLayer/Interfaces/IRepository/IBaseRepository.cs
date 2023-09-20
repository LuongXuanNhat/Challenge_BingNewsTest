using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T entity);
        Task<T> GetById(string id);
        Task<bool> Update(T entity);
        Task Delete(string id);
        Task<IEnumerable<T>> GetAll();
    }
}
