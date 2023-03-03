using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IUserService
    {
        public Task Register(User user);
        public Task<User> Login(string username, string password);

        public Task ChangePassword(User user);
    }
}
