using AutoMapper;
using Demo.BL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees = unitOfWork.EmployeeRepository.GetAll();
            else
                employees = unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            ViewData["Message"] = "Hello from viewData";
            ViewBag.Message = "Hello from viewBag";

            var mappedEmployeesVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployeesVM);

        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = Utility.UploadFile(employeeVM.Image, "Images");

                var mappedEmployee = mapper.Map<Employee>(employeeVM);
                unitOfWork.EmployeeRepository.Create(mappedEmployee);
                var result = unitOfWork.SaveChanges();
                if (result > 0)
                    TempData["Message"] = "Employee created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(employeeVM);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            ViewBag.Departments = unitOfWork.DepartmentRepository.GetAll();
            if (id is null)
                return BadRequest();
            var employee = unitOfWork.EmployeeRepository.Get(id.Value);
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
                    unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    unitOfWork.SaveChanges();
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
                unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                unitOfWork.SaveChanges();
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
