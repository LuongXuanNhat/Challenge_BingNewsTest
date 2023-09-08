using BingNew.BusinessLogicLayer.ModelConfig;
using BingNew.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Interfaces.IService
{
    public interface IArticleService : IBaseService<Article>
    {
        Task<bool> AddRange(IEnumerable<Article> articles);
        List<Article> UpdateArticlesFromTuoiTreNews(Config config);
    }
}
