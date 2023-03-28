using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IContractService
    {
        public Task ExtendsContractAsyn(Contract c);
        public Task<Contract> GetContractByRoomId(int roomId, int userId); 
        public Task<List<Contract>> getListContractsByUserId(int userId);
        public Task<bool> isMemberOfRoom(int roomId, int userId);
        public Task UpdateMoveContract(int roomId); 
    }
}
