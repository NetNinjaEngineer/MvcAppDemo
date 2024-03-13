using AutoMapper;
using Demo.BL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(_mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Create(mappedDepartment);
                var result = _unitOfWork.SaveChanges();
                if (result > 0)
                    TempData["Message"] = "Department created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(departmentVM);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department is null)
                return NotFound();
            var departmentVM = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, departmentVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
            => Details(id, "Edit");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(departmentVM);

        }

        [HttpGet]
        public IActionResult Delete(int id)
            => Details(id, "Delete");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            try
            {
                var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(department);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentVM);
            }

        }
    }
}
