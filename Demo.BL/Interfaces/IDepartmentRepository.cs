using Demo.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BL.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAll();

        Task<Department> GetById(int id);

        Task<int> Add(Department department);

        Task<int> Update(Department department);

        Task<int> Delete(Department department);
    }
}
