using Microsoft.EntityFrameworkCore;
using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class ContractRepository : BaseRepository<Contract>, IContractRepository
    {
        public ContractRepository(MotelManagementContext context) : base(context)
        {
        }

        public async Task<List<Contract>> getListContractsByUserId(int userId)
        {
            return await _context.Contracts.Where(c => c.UserId==userId)
                                    .Include(c=>c.Room)
                                        .ThenInclude(c=> c.Images)
                                    .ToListAsync();

        }

        public async Task<bool> IsMemberOfRoom(User user, int roomId)
        {
             Contract contract =  _context.Contracts.Where(c => c.RoomId == roomId 
                                                             && c.User.UserId == user.UserId 
                                                             && c.IsActive == true).FirstOrDefault();
            if(contract != null)
                return true;
            else
                return false;
        }
    }
}
