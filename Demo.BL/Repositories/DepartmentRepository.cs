using Demo.BL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MvcAppG01DbContext _context;

        public DepartmentRepository(MvcAppG01DbContext context) => _context = context;

        public async Task<int> Add(Department department)
        {
            _context.Add(department);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(Department department)
        {
            _context.Remove(department);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetAll()
            => await _context.Departments.ToListAsync();

        public async Task<Department> GetById(int id)
            => await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> Update(Department department)
        {
            _context.Departments.Update(department);
            return await _context.SaveChangesAsync();
        }
    }
}
