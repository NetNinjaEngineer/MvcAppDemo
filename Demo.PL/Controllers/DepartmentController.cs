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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _departmentRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.Add(department);
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = await _departmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(viewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
            => await Details(id, "Edit");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
            => await Details(id, "Delete");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        {
            await _departmentRepository.Delete(department);
            return RedirectToAction(nameof(Index));
        }
    }
}
