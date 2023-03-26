using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IPassingRepository : IGenericRepository<Passing>
    {
        public Task<bool> isBookingPassing(int? roomid, int userId);
    }
}
