using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IRoomTypeService
    {
       Task<List<RoomType>> GetAll();
    }
}
