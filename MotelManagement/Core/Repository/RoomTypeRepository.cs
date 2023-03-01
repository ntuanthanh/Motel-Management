﻿using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class RoomTypeRepository : BaseRepository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(MotelManagementContext context) : base(context)
        {

        }
        public void Update(RoomType category)
        {
            _context.Update(category);
        }
    }
}
