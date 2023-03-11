using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> isBooking(int? roomid, int userId)
        {
            return await _unitOfWork.bookingRepository.isBooking(roomid, userId); 
        }

        public async Task<List<Booking>> listBookings(int userId)
        {
            return await _unitOfWork.bookingRepository.listBookings(userId);
        }

        public async Task Register(int userId, int roomid)
        {
            Booking booking = new Booking();
            booking.RoomId = roomid;
            booking.UserId = userId;
            booking.Status = (int)REGISTER_ROOM_STATE.REGISTER;
            booking.BookingTime = DateTime.Now;
            booking.MeetingDate = null; 
            await _unitOfWork.bookingRepository.AddAsync(booking);
            await _unitOfWork.SaveAsync();
        }

        public async Task updateUnRegister(int userId, int roomid)
        {
            await _unitOfWork.bookingRepository.updateUnRegister(userId, roomid);
            await _unitOfWork.SaveAsync();
        }
    }
}
