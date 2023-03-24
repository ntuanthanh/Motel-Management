using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        // Viết thêm các hàm muốn repo xử lý ở dưới ngoài các CRUD trong IGenericRepository
       Task<List<Room>> advancedSearchRoom(string nameRoom, int? roomTypeId, int? status, decimal fromPrice, decimal toPrice, int fromSizePerson, int toSizePerson, int offSet, int count);
       Task<int> Count(string nameRoom, int? roomTypeId, int? status, decimal fromPrice, decimal toPrice, int fromSizePerson, int toSizePerson);
       Task<List<Room>> Top4BestRoom();
       Task<Room> getRoomById(int id);
       Task<List<Room>> RoomSimilar(int? status);
       Task UpdateStatusRoom(int roomId, int status);
    }
}
