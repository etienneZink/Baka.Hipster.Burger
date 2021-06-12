using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class OrderRepository: IOrderRepository
    {

        private readonly INHibernateHelper _nHibernateHelper;

        public OrderRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(Order order)
        {
            if (order is null) return -1;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.SaveOrUpdateAsync(order);
                await transaction.CommitAsync();

                return session.QueryOver<Order>()
                    .Where(o => o.OrderNumber == order.OrderNumber)
                    .SingleOrDefaultAsync<Order>()
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
                var orderToDelete = await session.QueryOver<Order>()
                    .Where(o => o.Id == id)
                    .SingleOrDefaultAsync<Order>();
                if (orderToDelete is null) return false;

                await session.DeleteAsync(orderToDelete);
                await transaction.CommitAsync();

                return (session.QueryOver<Order>()
                    .Where(o => o.Id == id)
                    .SingleOrDefaultAsync<Order>() is null);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<Order> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<Order>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<Order>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task <ICollection<Order>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<Order>()
                    .ListAsync<Order>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}