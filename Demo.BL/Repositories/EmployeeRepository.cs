using Demo.BL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using System.Linq;

namespace Demo.BL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcAppG01DbContext _context;
        public EmployeeRepository(MvcAppG01DbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
            => _context.Employees.Where(x => x.Address == address);
    }
}
