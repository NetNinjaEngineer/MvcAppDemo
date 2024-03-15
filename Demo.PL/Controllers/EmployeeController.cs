using AutoMapper;
using Demo.BL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string SearchValue, string sortOrder)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees = await unitOfWork.EmployeeRepository.GetAllAsync();
            else
                employees = unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            ViewData[Constants.NameSortParam] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData[Constants.AgeSortParam] = sortOrder == "Age" ? "age_desc" : "Age";
            ViewData[Constants.SalarySortParam] = sortOrder == "Salary" ? "salary_desc" : "salary";
            ViewData[Constants.EmailSortParam] = sortOrder == "Email" ? "email_desc" : "email";
            ViewData[Constants.AddressSortParam] = sortOrder == "Address" ? "address_desc" : "address";
            ViewData[Constants.HireDateSortParam] = sortOrder == "HireDate" ? "hireDate_desc" : "hireDate";

            employees = Sort(sortOrder, employees);

            ViewData["Message"] = "Hello from viewData";
            ViewBag.Message = "Hello from viewBag";

            IEnumerable<EmployeeViewModel> mappedEmployeesVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployeesVM);
        }

        private static IEnumerable<Employee> Sort(string sortOrder, IEnumerable<Employee> employees)
        {
            employees = sortOrder switch
            {
                "name_desc" => employees.OrderByDescending(e => e.Name),
                "age_desc" => employees.OrderByDescending(e => e.Age),
                "Age" => employees.OrderBy(e => e.Age),
                "salary" => employees.OrderBy(e => e.Salary),
                "salary_desc" => employees.OrderByDescending(e => e.Salary),
                "address" => employees.OrderBy(e => e.Address),
                "address_desc" => employees.OrderByDescending(e => e.Address),
                "email" => employees.OrderBy(e => e.Address),
                "email_desc" => employees.OrderByDescending(e => e.Address),
                "hireDate_desc" => employees.OrderByDescending(e => e.HireDate),
                "hireDate" => employees.OrderBy(e => e.HireDate),
                _ => employees.OrderBy(e => e.Name),
            };
            return employees;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await PassDepartmentsToViewsPartial();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                if (employeeVM.Image is not null && employeeVM.Image.Length > 0)
                    employeeVM.ImageName = Utility.UploadFile(employeeVM.Image, "Images");

                var mappedEmployee = mapper.Map<Employee>(employeeVM);
                await unitOfWork.EmployeeRepository.CreateAsync(mappedEmployee);
                var result = await unitOfWork.SaveChangesAsync();
                if (result > 0)
                    TempData["Message"] = "Employee created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(employeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            ViewBag.Departments = await PassDepartmentsToViewsPartial();
            if (id is null)
                return BadRequest();
            var employee = await unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var employeeVM = mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee is null)
                return NotFound();
            return View(viewName, employeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
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
                    await unitOfWork.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(employeeVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmployee = mapper.Map<Employee>(employeeVM);
                unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                var result = await unitOfWork.SaveChangesAsync();
                if (result > 0)
                    Utility.DeleteFile(employeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }

        }

        private async Task<IEnumerable<Department>> PassDepartmentsToViewsPartial()
        {
            var departments = await unitOfWork.DepartmentRepository.GetAllAsync();
            return departments;
        }
    }
}
