using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class UserRepository: IUserRepository
    {

        private readonly INHibernateHelper _nHibernateHelper;

        public UserRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(User user)
        {
            if (user is null) return -1;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.SaveOrUpdateAsync(user);
                await transaction.CommitAsync();

                return session.QueryOver<User>()
                    .Where(u => u.Username == user.Username)
                    .SingleOrDefaultAsync<User>()
                    .Id;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return -1;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            try
            {
                var areaToDelete = await session.QueryOver<User>()
                    .Where(u => u.Id == id)
                    .SingleOrDefaultAsync<User>();
                if (areaToDelete is null) return false;

                await session.DeleteAsync(areaToDelete);
                await transaction.CommitAsync();

                return (session.QueryOver<User>()
                    .Where(u => u.Id == id)
                    .SingleOrDefaultAsync<User>() is null);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<User> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<User>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<User>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ICollection<User>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<User>()
                    .ListAsync<User>();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}