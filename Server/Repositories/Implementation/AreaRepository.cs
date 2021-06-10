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

        public async Task<bool> NewOrUpdate(Area area)
        {
            if (area is null) return false;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            session.SaveOrUpdateAsync(area);

            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Area> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Area>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}