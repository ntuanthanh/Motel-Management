using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        void AddUserRegister(User user);
    }
}
