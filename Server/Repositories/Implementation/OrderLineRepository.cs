using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class OrderLineRepository: IOrderLineRepository
    {
        private readonly INHibernateHelper _nHibernateHelper;

        public OrderLineRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(OrderLine orderLine)
        {
            if (orderLine is null) return -1;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.SaveOrUpdateAsync(orderLine);
                await transaction.CommitAsync();

                return orderLine.Id;
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
                var orderLineToDelete = await session.QueryOver<OrderLine>()
                    .Where(o => o.Id == id)
                    .SingleOrDefaultAsync<OrderLine>();
                if (orderLineToDelete is null) return false;

                await session.DeleteAsync(orderLineToDelete);
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<OrderLine> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<OrderLine>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<OrderLine>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ICollection<OrderLine>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<OrderLine>()
                    .ListAsync<OrderLine>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}