using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IBookingRepository
    {
        public Task<bool> isBooking(int? roomid, int userId);
        public Task<List<Booking>> listBookings(int userId);
        Task updateUnRegister(int userId, int roomid);
    }
}
