using MotelManagement.Business.IService;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RoomType>> GetAll()
        {
            return await _unitOfWork.roomTypeRepository.GetAllAsync(); 
        }
        public RoomType addNewRoomType(RoomType newRoomType)
        {
            // Business các thứ ....
            
            // Add 
           _unitOfWork.roomTypeRepository.Add(newRoomType);
           // Dùng Save của UnitOfWork to save change 
           _unitOfWork.save();
            return newRoomType;
        }
    }
}
