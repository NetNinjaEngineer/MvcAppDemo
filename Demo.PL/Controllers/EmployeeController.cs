using AutoMapper;
using Demo.BL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees = employeeRepository.GetAll();
            else
                employees = employeeRepository.GetEmployeesByName(SearchValue);

            ViewData["Message"] = "Hello from viewData";
            ViewBag.Message = "Hello from viewBag";

            var mappedEmployeesVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployeesVM);

        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var mappedEmployee = mapper.Map<Employee>(employeeVM);
                var result = employeeRepository.Create(mappedEmployee);
                if (result > 0)
                    TempData["Message"] = "Employee created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(employeeVM);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = employeeRepository.Get(id.Value);
            var employeeVM = mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee is null)
                return NotFound();
            return View(viewName, employeeVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
            => Details(id, "Edit");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = mapper.Map<Employee>(employeeVM);
                    employeeRepository.Update(mappedEmployee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(employeeVM);

        }

        [HttpGet]
        public IActionResult Delete(int id)
            => Details(id, "Delete");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmployee = mapper.Map<Employee>(employeeVM);
                employeeRepository.Delete(mappedEmployee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }

        }
    }
}
