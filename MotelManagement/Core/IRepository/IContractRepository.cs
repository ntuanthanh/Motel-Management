using MotelManagement.Data.Models;
using System.Collections;

namespace MotelManagement.Core.IRepository
{
    public interface IContractRepository
    {
        public Task<List<Contract>> getListContractsByUserId(int userId);
        public Task<bool> IsMemberOfRoom(User user, int roomId);
        public Task<Hashtable> getOwners(int roomId, int userId);
    }

}
