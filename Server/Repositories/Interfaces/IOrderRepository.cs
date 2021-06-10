using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> NewOrUpdate(Order order);
        Task<bool> Delete(int id);
        Task<Order> Get(int id);
        Task<ICollection<Order>> GetAll();
    }
}