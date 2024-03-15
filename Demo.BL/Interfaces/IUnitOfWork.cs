using System.Threading.Tasks;

namespace Demo.BL.Interfaces
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; set; }

        public IDepartmentRepository DepartmentRepository { get; set; }

        Task<int> SaveChangesAsync();
    }
}
