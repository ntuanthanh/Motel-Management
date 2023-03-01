﻿using MotelManagement.Core.IRepository;
using MotelManagement.Data.Models;

namespace MotelManagement.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MotelManagementContext _context;

        public IRoomTypeRepository roomTypeRepository { get; }

        public UnitOfWork(MotelManagementContext context)
        {
            _context = context;
            roomTypeRepository = new RoomTypeRepository(_context);
        }
        public void Dispose()
        {
            _context.Dispose();
            // Giải phóng vùng nhớ
            GC.SuppressFinalize(this);
        }
        public void save()
        {
            _context.SaveChanges();
        }
    }
}
