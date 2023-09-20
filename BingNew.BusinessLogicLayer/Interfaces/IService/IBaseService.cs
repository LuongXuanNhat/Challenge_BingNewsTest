using BingNew.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<T> GetById(string id);
        Task<bool> Update(T entity);
        Task<bool> Delete(string id);
        Task<IEnumerable<T>> GetAll();
    }
}
