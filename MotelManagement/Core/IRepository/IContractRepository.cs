using MotelManagement.Data.Models;

namespace MotelManagement.Core.IRepository
{
    public interface IContractRepository
    {
        public Task<List<Contract>> getListContractsByUserId(int userId);
    }
}
