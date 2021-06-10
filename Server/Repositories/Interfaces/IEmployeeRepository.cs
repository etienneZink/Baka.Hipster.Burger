using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> NewOrUpdate(Employee employee);
        Task<bool> Delete(int id);
        Task<Employee> Get(int id);
        Task<ICollection<Employee>> GetAll();
    }
}