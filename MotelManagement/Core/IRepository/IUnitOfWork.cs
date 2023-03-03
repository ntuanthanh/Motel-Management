using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        // Inteface IRepository
        IRoomTypeRepository roomTypeRepository { get; }
        IUserRepository userRepository { get; }
        void save();

        Task SaveAsync();
    }
}
