using Demo.DAL.Models;
using System.Linq;

namespace Demo.BL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByAddress(string address);
        IQueryable<Employee> GetEmployeesByName(string empName);
    }
}
