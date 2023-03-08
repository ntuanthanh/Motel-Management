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
                                    .Include(c=>c.Room).ToListAsync();

        }
    }
}
