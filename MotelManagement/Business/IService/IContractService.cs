using MotelManagement.Data.Models;

namespace MotelManagement.Business.IService
{
    public interface IContractService
    {
        public Task<List<Contract>> getListContractsByUserId(int userId);
    }
}
