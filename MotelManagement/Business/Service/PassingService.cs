using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class PassingService : IPassingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PassingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> isBookingPassing(int? roomid, int userId)
        {
            return await _unitOfWork.passingRepository.isBookingPassing(roomid, userId);
        }

        public async Task<bool> isRoomRented(int? roomId)
        {
            return await _unitOfWork.roomRepository.isRoomRentedReal(roomId);
        }
        public async Task<bool> isRoomWaiting(int? roomId)
        {
            return await _unitOfWork.roomRepository.isRoomWaiting(roomId);
        }

        public async Task<List<Passing>> PassingBookingsAvailableSearching(int? roomId, string? nameBooking, string? emailBooking, string? phoneBooking, DateTime? fromBooking, DateTime? toBooking)
        {
            List<Passing> bookings = await _unitOfWork.passingRepository.PassingBookingListByRoomAvailableSearching(roomId, nameBooking, emailBooking, phoneBooking, fromBooking, toBooking);
            return bookings;
        }

        public async Task RegisterPassing(int userId, int roomid)
        {
            // roomId
            // userId là ReuqestUser : người gửi yêu cầu muốn đăng kí đến member
            int member; // không cần, đang set trong trường là null 

            Passing passing = new Passing();
            passing.RoomId = roomid;
            passing.UserRequestId = userId;
            passing.Status = (int)REGISTER_ROOM_STATE.REGISTER;
            passing.BookingTime = DateTime.Now;
            passing.MeetingDate = null;
            passing.MemberId = null; 
            await _unitOfWork.passingRepository.AddAsync(passing); 
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateMeetingDateAllUser(DateTime? meetingDate, int? roomId)
        {
            await _unitOfWork.passingRepository.updateMeetingDate(meetingDate, roomId);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateMeetingForUser(DateTime? dateTime, int? bookingId, int? roomId)
        {
            await _unitOfWork.passingRepository.updateMeetingDateForUser(dateTime, bookingId);
            await _unitOfWork.SaveAsync();
        }
        public async Task PushUserToAdmin(int userId, int roomid, int bookingId)
        {
            // Update status Booking 
            await _unitOfWork.passingRepository.updateSuccessRejectUsersExceptMember(userId, roomid, bookingId);
            //// Update status Room
            await _unitOfWork.roomRepository.UpdateStatusRoom(roomid, (int)ROOM_STATE.Waiting);
            // Save transaction 
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<Passing>> PassingsWaitingSearching(string? roomBooking, string? nameBooking, string? emailBooking, string? fromBooking, string? toBooking)
        {
            List<Passing> passings = await _unitOfWork.passingRepository.PassingListByRoomWaitingSearching(roomBooking, nameBooking, emailBooking, fromBooking, toBooking);
            return passings;
        }

        public async Task SetUserBeMember(int userId, int roomid, decimal price, int bookingId)
        {
            // Update Status Old Contract
            await _unitOfWork.contractRepository.UpdateMoveContract(roomid); 
            // Add to Contract 
            await _unitOfWork.contractRepository.addUsertoRoom(roomid, userId, price);
            // Update status Booking  
            await _unitOfWork.passingRepository.updateStatusPassing(userId, roomid, bookingId, (int)REGISTER_ROOM_STATE.SUCCESS); // Success
            // Update status Room 
            await _unitOfWork.roomRepository.UpdateStatusRoom(roomid, (int)ROOM_STATE.RENTED);
            // Save transaction 
            await _unitOfWork.SaveAsync();
        }

        public async Task RejectPassing(int userId, int roomid, int bookingId)
        {
            // Update status Booking 
            await _unitOfWork.passingRepository.updateStatusPassing(userId, roomid, bookingId, (int)REGISTER_ROOM_STATE.REJECT); // Success
            // Update status Room
            await _unitOfWork.roomRepository.UpdateStatusRoom(roomid, (int)ROOM_STATE.PASSING);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<Passing>> GetListPassings(int userId)
        {
            return await _unitOfWork.passingRepository.GetListPassings(userId);
        }

        public async Task<bool> isPassing(int roomid, int userId)
        {
            return await _unitOfWork.passingRepository.isPassing(roomid, userId);
        }

        public async Task UpdateUnregisterPassing(int userId, int roomid)
        {
            await _unitOfWork.passingRepository.UpdateUnregisterPassing(userId, roomid);
            await _unitOfWork.SaveAsync();
        }
    }
}
