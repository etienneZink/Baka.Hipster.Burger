using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface IAreaRepository
    {
        Task<bool> NewOrUpdate(Area area);
        Task<bool> Delete(int id);
        Task<Area> Get(int id);
        Task<ICollection<Area>> GetAll();
    }
}