using Demo.BL.Interfaces;
using Demo.DAL.Models;
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
            return View(await _departmentRepository.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.Add(department);
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = await _departmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = await _departmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (department is null)
                return BadRequest();

            var existDepartment = await _departmentRepository.GetById(department.Id);
            if (existDepartment is null)
                return NotFound();
            existDepartment.Code = department.Code;
            existDepartment.DateOfCreation = department.DateOfCreation;
            existDepartment.Name = department.Name;

            await _departmentRepository.Update(existDepartment);
            return RedirectToAction(nameof(Index));
        }
    }
}
