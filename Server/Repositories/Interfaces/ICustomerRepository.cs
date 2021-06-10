using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<bool> NewOrUpdate(Customer customer);
        Task<bool> Delete(int id);
        Task<Customer> Get(int id);
        Task<ICollection<Customer>> GetAll();
    }
}