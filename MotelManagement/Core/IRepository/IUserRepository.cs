using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> Authentication(string username, string password);
        void ChangePassword(User user);

    }
}
