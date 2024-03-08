using Demo.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
            => _departmentRepository = departmentRepository;

        public async Task<IActionResult> Index()
        {
            return Ok(await _departmentRepository.GetAll());
        }
    }
}
