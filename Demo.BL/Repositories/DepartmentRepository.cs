using Demo.BL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;

namespace Demo.BL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MvcAppG01DbContext context) : base(context)
        {
        }
    }
}
