using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IRoomService
    {
        public Task<List<Room>> advancedSearchRoomList(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson, int page);
        public Task<int> totalPage(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson);

        public Task<List<Room>> roomRecommended();
        public Task<Room> getRoomById(int roomId);
        public Task<List<Room>> roomSimilar(int ? statusRoom);
        public Task PassingRoom(Room room);
        public Task<bool> isRoomRented(string roomName);
    }
}
