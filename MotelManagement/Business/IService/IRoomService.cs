using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IRoomService
    {
        public Task<List<Room>> advancedSearchRoomList(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson, int page);
        public Task<int> totalPage(string nameRoom, int? roomTypeId, int? status, string price, string sizePerson);

        public Task<List<Room>> roomRecommended(); 
    }
}
