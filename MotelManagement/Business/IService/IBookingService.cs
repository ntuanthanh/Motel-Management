using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IBookingService
    {
        public Task<bool> isBooking(int? roomid, int userId);
        public Task<List<Booking>> listBookings(int userId);

        public Task updateUnRegister(int userId, int roomid);
        public Task Register(int userId, int roomid);
        public Task<List<Booking>> BookingsAvailable(int? roomId);
    }
}
