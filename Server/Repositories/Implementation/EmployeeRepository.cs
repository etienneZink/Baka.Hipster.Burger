using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class EmployeeRepository: IEmployeeRepository
    {
        public async Task<int> NewOrUpdate(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Employee>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}