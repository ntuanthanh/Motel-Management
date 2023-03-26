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
    }
}
