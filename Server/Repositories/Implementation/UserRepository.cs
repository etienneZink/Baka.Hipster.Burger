using System.Collections.Generic;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;

namespace Baka.Hipster.Burger.Server.Repositories.Implementation
{
    public class UserRepository: IUserRepository
    {
        public async Task<int> NewOrUpdate(User user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}