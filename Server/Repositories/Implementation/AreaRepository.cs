using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class AreaRepository: IAreaRepository
    {

        private readonly INHibernateHelper _nHibernateHelper;

        public AreaRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(Area area)
        {
            if (area is null) return -1;

            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.SaveOrUpdateAsync(area);
                await transaction.CommitAsync();

                return session.QueryOver<Area>()
                    .Where(a => a.PostCode == area.PostCode)
                    .SingleOrDefaultAsync<Area>()
                    .Id;
            }
            catch (Exception)
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
                var areaToDelete = await session.QueryOver<Area>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<Area>();
                if (areaToDelete is null) return false;

                await session.DeleteAsync(areaToDelete);
                await transaction.CommitAsync();

                return (session.QueryOver<Area>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<Area>() is null);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<Area> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<Area>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<Area>();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<ICollection<Area>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<Area>()
                    .ListAsync<Area>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}