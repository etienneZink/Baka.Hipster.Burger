using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class ArticleRepository: IArticleRepository
    {
        public async Task<bool> NewOrUpdate(Article article)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Article> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Article>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}