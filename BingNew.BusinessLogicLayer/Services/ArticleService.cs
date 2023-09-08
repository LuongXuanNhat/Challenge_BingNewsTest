using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace BingNew.BusinessLogicLayer.Services
{
    public class ArticleService : IBaseService<Article>
    {
        private readonly ArticleRepository _articleRepository;
        public ArticleService(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }   
        public async Task<bool> Add(Article entity)
        {
            try
            {
               await _articleRepository.Add(entity);

            } catch (Exception e) {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.Message.ToString());
                return false;
            }
            return true;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Article>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Article> GetById(string id)
        {
            try
            {
                var result = await _articleRepository.GetById(id);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BUG KÌA, FIX ĐI: " + e.ToString());
                return new Article();
            }
        }

        public Task<bool> Update(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
