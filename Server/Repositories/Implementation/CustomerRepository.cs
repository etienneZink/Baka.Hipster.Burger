using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class CustomerRepository: ICustomerRepository
    {
        public async Task<bool> NewOrUpdate(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Customer> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Customer>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}