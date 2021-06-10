using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class OrderRepository: IOrderRepository
    {
        public async Task<int> NewOrUpdate(Order order)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Order> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task <ICollection<Order>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}