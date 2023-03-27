using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IPassingService
    {
        public Task<bool> isBookingPassing(int? roomid, int userId);

        public Task RegisterPassing(int userId, int roomid);
        public Task<List<Passing>> PassingBookingsAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, DateTime? fromBooking, DateTime? toBooking);
        public Task UpdateMeetingDateAllUser(DateTime? dateTime, int? roomId);
        public Task UpdateMeetingForUser(DateTime? dateTime, int? bookingId, int? roomId);
        public Task PushUserToAdmin(int userId, int roomid, int bookingId);
        public Task<bool> isRoomRented(int? roomId);
        public Task<bool> isRoomWaiting(int? roomId);
        public Task<List<Passing>> PassingsWaitingSearching(string? roomBooking, string? nameBooking, string? emailBooking, string? fromBooking, string? toBooking);
        public Task SetUserBeMember(int userId, int roomid, decimal price, int bookingId);
        public Task RejectPassing(int userId, int roomid, int bookingId);
        public Task<List<Passing>> GetListPassings(int userId);
        public Task<bool> isPassing(int roomid, int userId);
        public Task UpdateUnregisterPassing(int userId, int roomid);
    }
}
