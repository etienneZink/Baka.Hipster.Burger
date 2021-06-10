﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class CustomerRepository: ICustomerRepository
    {

        private readonly INHibernateHelper _nHibernateHelper;

        public CustomerRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(Customer customer)
        {
            if (customer is null) return -1;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            await session.SaveOrUpdateAsync(customer);
            await transaction.CommitAsync();

            return session.QueryOver<Customer>()
                .Where(c => c.Phone == customer.Phone)
                .SingleOrDefaultAsync<Customer>()
                .Id;
        }

        public async Task<bool> Delete(int id)
        {
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            var customerToDelete = await session.QueryOver<Customer>()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync<Customer>();
            if (customerToDelete is null) return false;

            await session.DeleteAsync(customerToDelete);
            await transaction.CommitAsync();

            return (session.QueryOver<Customer>()
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync<Customer>() is null);
        }

        public async Task<Customer> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            return await session.QueryOver<Customer>()
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync<Customer>();
        }

        public async Task<ICollection<Customer>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            return await session.QueryOver<Customer>()
                .ListAsync<Customer>();
        }
    }
}