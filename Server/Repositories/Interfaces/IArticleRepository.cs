using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<bool> NewOrUpdate(Article article);
        Task<bool> Delete(int id);
        Task<Article> Get(int id);
        Task<ICollection<Article>> GetAll();
    }
}