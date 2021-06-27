using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class EmployeeRepository: IEmployeeRepository
    {

        private readonly INHibernateHelper _nHibernateHelper;

        public EmployeeRepository(INHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }

        public async Task<int> NewOrUpdate(Employee employee)
        {
            if (employee is null) return -1;
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            try
            {
                await session.SaveOrUpdateAsync(employee);
                await transaction.CommitAsync();

                return employee.Id;
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
                var employeeToDelete = await session.QueryOver<Employee>()
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync<Employee>();
                if (employeeToDelete is null) return false;

                await session.DeleteAsync(employeeToDelete);
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<Employee> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<Employee>()
                    .Where(a => a.Id == id)
                    .SingleOrDefaultAsync<Employee>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ICollection<Employee>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            try
            {
                return await session.QueryOver<Employee>()
                    .ListAsync<Employee>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}