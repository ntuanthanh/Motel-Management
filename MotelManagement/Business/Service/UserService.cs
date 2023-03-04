using MotelManagement.Business.IService;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Business.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChangePasswordAync(User user)
        {
             _unitOfWork.userRepository.ChangePassword(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<User> Login(string username, string password)
        {
            return await _unitOfWork.userRepository.Authentication(username, password);
        }

        public async Task Register(User user)
        {
            //_unitOfWork.save(user);
            await _unitOfWork.userRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
