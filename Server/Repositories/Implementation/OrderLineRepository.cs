using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class OrderLineRepository: IOrderLineRepository
    {
        public async Task<int> NewOrUpdate(OrderLine orderLine)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<OrderLine> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<OrderLine>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}