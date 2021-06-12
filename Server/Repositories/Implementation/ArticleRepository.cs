using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class ArticleRepository: IArticleRepository
    {

        private readonly INHibernateHelper _nHibernateHelper;

        public ArticleRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(Article article)
        {
            if (article is null) return -1;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            await session.SaveOrUpdateAsync(article);
            await transaction.CommitAsync();

            return session.QueryOver<Article>()
                .Where(a => a.ArticleNumber == article.ArticleNumber)
                .SingleOrDefaultAsync<Article>()
                .Id;
        }

        public async Task<bool> Delete(int id)
        {
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            var articleToDelete = await session.QueryOver<Article>()
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync<Article>();
            if (articleToDelete is null) return false;

            await session.DeleteAsync(articleToDelete);
            await transaction.CommitAsync();

            return (session.QueryOver<Article>()
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync<Article>() is null);
        }

        public async Task<Article> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            return await session.QueryOver<Article>()
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync<Article>();
        }

        public async Task<ICollection<Article>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            return await session.QueryOver<Article>()
                .ListAsync<Article>();
        }
    }
}