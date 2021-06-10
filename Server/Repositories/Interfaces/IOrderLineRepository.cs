using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface IOrderLineRepository
    {
        Task<bool> NewOrUpdate(OrderLine orderLine);
        Task<bool> Delete(int id);
        Task<OrderLine> Get(int id);
        Task<ICollection<OrderLine>> GetAll();
    }
}