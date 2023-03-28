using MotelManagement.Business.IService;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExtendsContractAsyn(Contract c)
        {
            await _unitOfWork.contractRepository.ExtendsContractAsyn(c);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Contract> GetContractByRoomId(int roomId, int userId)
        {
            return await _unitOfWork.contractRepository.GetContractByRoomId(roomId, userId);
        }

        public async Task<List<Contract>> getListContractsByUserId(int userId)
        {
            return await _unitOfWork.contractRepository.getListContractsByUserId(userId);

        }

        public async Task<bool> isMemberOfRoom(int roomId, int userId)
        {
            User user = new User(); 
            user.UserId = userId;
            return await _unitOfWork.contractRepository.IsMemberOfRoom(user, roomId);
        }

        public async Task UpdateMoveContract(int roomId)
        {
            await _unitOfWork.contractRepository.UpdateMoveContract(roomId);
            await _unitOfWork.SaveAsync();
        }
    }
}
