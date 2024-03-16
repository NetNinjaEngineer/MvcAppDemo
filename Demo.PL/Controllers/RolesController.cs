using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
                IEnumerable<RoleViewModel> mappedRoles = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(roles);
                return View(mappedRoles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(searchValue);
                if (role is null)
                {
                    return View();
                }

                var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);
                return View(new List<RoleViewModel> { mappedRole });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityRole mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                    if (await _roleManager.RoleExistsAsync(mappedRole.Name))
                    {
                        ModelState.AddModelError(string.Empty, "Role exists !!!");
                    }
                    IdentityResult result = await _roleManager.CreateAsync(mappedRole);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View(model);
        }


        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            RoleViewModel mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(viewName, mappedRole);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (model.RoleId != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    IdentityRole role = await _roleManager.FindByIdAsync(model.RoleId);
                    role.Name = model.RoleName;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    IdentityResult result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Role Deleted Successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

    }
}
