using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.DataAccessLayer.Models;
using BingNew.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
                Console.WriteLine(e.ToString());
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

        public Task<Article> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
