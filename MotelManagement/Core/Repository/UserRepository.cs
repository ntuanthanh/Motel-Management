using Microsoft.EntityFrameworkCore;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MotelManagementContext context) : base(context)
        {
        }

        public void Add(User entity)
        {
            _context.Add(entity);
        }

        public async Task<User> Authentication(string username, string password)
        {
            return await _context.Users.Where(user => user.Password.Equals(password)
                                &&user.Email.Equals(user)).FirstOrDefaultAsync();

        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {

        }
    }
}
