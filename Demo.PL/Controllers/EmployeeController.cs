using AutoMapper;
using Demo.BL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult Index(string SearchValue, string sortOrder)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees = unitOfWork.EmployeeRepository.GetAll();
            else
                employees = unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "age_desc" : "Age";

            employees = sortOrder switch
            {
                "name_desc" => employees.OrderByDescending(e => e.Name),
                "age_desc" => employees.OrderByDescending(e => e.Age),
                _ => employees.OrderBy(e => e.Name),
            };
            ViewData["Message"] = "Hello from viewData";
            ViewBag.Message = "Hello from viewBag";

            var mappedEmployeesVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployeesVM);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = PassDepartmentsToViewsPartial();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                if (employeeVM.Image is not null && employeeVM.Image.Length > 0)
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
            ViewBag.Departments = PassDepartmentsToViewsPartial();
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
        {
            return Details(id, "Edit");
        }

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
                    if (employeeVM.Image?.Length > 0 && employeeVM.Image is not null)
                    {
                        var imageName = Utility.UploadFile(employeeVM.Image, "Images");
                        employeeVM.ImageName = imageName;
                    }

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
        {
            return Details(id, "Delete");
        }

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
                var result = unitOfWork.SaveChanges();
                if (result > 0)
                    Utility.DeleteFile(employeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }

        }

        private IEnumerable<Department> PassDepartmentsToViewsPartial()
        {
            var departments = unitOfWork.DepartmentRepository.GetAll();
            return departments;
        }
    }
}
