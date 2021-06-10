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

            await session.SaveOrUpdateAsync(employee);
            await transaction.CommitAsync();

            return session.QueryOver<Employee>()
                .Where( e=> e.EmployeeNumber == employee.EmployeeNumber)
                .SingleOrDefaultAsync<Employee>()
                .Id;
        }

        public async Task<bool> Delete(int id)
        {
            using var session = _nHibernateHelper.OpenSession();
            using var transaction = session.BeginTransaction();

            var employeeToDelete = await session.QueryOver<Employee>()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync<Employee>();
            if (employeeToDelete is null) return false;

            await session.DeleteAsync(employeeToDelete);
            await transaction.CommitAsync();

            return (session.QueryOver<Employee>()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync<Employee>() is null);
        }

        public async Task<Employee> Get(int id)
        {
            using var session = _nHibernateHelper.OpenSession();

            return await session.QueryOver<Employee>()
                .Where(a => a.Id == id)
                .SingleOrDefaultAsync<Employee>();
        }

        public async Task<ICollection<Employee>> GetAll()
        {
            using var session = _nHibernateHelper.OpenSession();

            return await session.QueryOver<Employee>()
                .ListAsync<Employee>();
        }
    }
}