using MotelManagement.Business.IService;
using MotelManagement.Common;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Room>> advancedSearchRoomList(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson, int page)
        {
            // Business 
            int pageSize = (int)PageManagement.PageSize;

            int offSet = (page - 1) * pageSize + 1;

            decimal fromPrice, toPrice;
            int fromMaxPerson, toMaxPerson;

            // Xử lý khoảng của price (1000000,200000)
            if (price == null || price.Equals(string.Empty))
            {
                fromPrice = 1000000;
                toPrice = 4000000;
            }
            else
            {
                string[] priceParts = price.Split(',');
                fromPrice = decimal.Parse(priceParts[0]);
                toPrice = decimal.Parse(priceParts[1]);
            }
            // Xử lý khoảng của sizePerson(1, 2)
            if (sizePerson == null || sizePerson.Equals(String.Empty))
            {
                fromMaxPerson = 1;
                toMaxPerson = 4;
            }
            else
            {
                string[] sizeParts = sizePerson.Split(',');
                fromMaxPerson = Int32.Parse(sizeParts[0]);
                toMaxPerson = Int32.Parse(sizeParts[1]);
            }
            // Query : 
            return await _unitOfWork.roomRepository.advancedSearchRoom(nameRoom, roomTypeId, status, fromPrice, toPrice, fromMaxPerson, toMaxPerson, offSet, pageSize);
        }
        public async Task<int> totalPage(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson)
        {
            // page size
            int pageSize = (int)PageManagement.PageSize;
            // Count tổng bản ghi 
            decimal fromPrice, toPrice;
            int fromMaxPerson, toMaxPerson;

            // Xử lý khoảng của price (1000000,200000)
            if (price == null || price.Equals(string.Empty))
            {
                fromPrice = 1000000;
                toPrice = 4000000;
            }
            else
            {
                string[] priceParts = price.Split(',');
                fromPrice = decimal.Parse(priceParts[0]);
                toPrice = decimal.Parse(priceParts[1]);
            }
            // Xử lý khoảng của sizePerson(1, 2)
            if (sizePerson == null || sizePerson.Equals(String.Empty))
            {
                fromMaxPerson = 1;
                toMaxPerson = 4;
            }
            else
            {
                string[] sizeParts = sizePerson.Split(',');
                fromMaxPerson = Int32.Parse(sizeParts[0]);
                toMaxPerson = Int32.Parse(sizeParts[1]);
            }
            int count = await _unitOfWork.roomRepository.Count(nameRoom, roomTypeId, status, fromPrice, toPrice, fromMaxPerson, toMaxPerson);
            return (count % pageSize == 0) ? (count / pageSize) : (count / pageSize) + 1;
        }
        public Task<List<Room>> roomRecommended()
        {
            return _unitOfWork.roomRepository.Top4BestRoom(); 
        }

        public Task<Room> getRoomById(int? roomId)
        {
            // Business get Room By Id 
            return _unitOfWork.roomRepository.getRoomById(roomId); 
        }

        public Task<List<Room>> roomSimilar(int? statusRoom)
        {
            return _unitOfWork.roomRepository.RoomSimilar(statusRoom);
        }
    }
}
