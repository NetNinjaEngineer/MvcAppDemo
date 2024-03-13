using Demo.BL.Interfaces;
using Demo.DAL.Context;
using System;

namespace Demo.BL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MvcAppG01DbContext _context;

        public UnitOfWork(MvcAppG01DbContext context)
        {
            _context = context;
            EmployeeRepository ??= new EmployeeRepository(_context);
            DepartmentRepository ??= new DepartmentRepository(_context);
        }

        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        public void Dispose() => _context.Dispose();

        public int SaveChanges() => _context.SaveChanges();
    }
}
