using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> NewOrUpdate(User user);
        Task<bool> Delete(int id);
        Task<User> Get(int id);
        Task<ICollection<User>> GetAll();
    }
}