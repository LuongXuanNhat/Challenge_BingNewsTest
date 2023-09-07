using BingNew.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces
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
