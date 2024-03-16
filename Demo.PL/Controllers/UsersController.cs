using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                IEnumerable<ApplicationUser> users = await userManager.Users.ToListAsync();
                IEnumerable<UserViewModel> mappedUsers = users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FName = user.FirstName,
                    Email = user.Email,
                    LName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userManager.GetRolesAsync(user).Result
                });

                return View(mappedUsers);

            }
            else
            {
                ApplicationUser user = await userManager.FindByEmailAsync(searchValue);

                IList<string> userRoles = await userManager.GetRolesAsync(user);
                UserViewModel mappedApplicationUser = new UserViewModel
                {
                    Email = user.Email,
                    FName = user.FirstName,
                    Id = user.Id,
                    LName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userRoles.ToList()
                };

                return View(new List<UserViewModel> { mappedApplicationUser });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser is null)
                return NotFound();

            UserViewModel MappedAppUser = mapper.Map<ApplicationUser, UserViewModel>(applicationUser);
            return View(viewName, MappedAppUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
        {
            if (model.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser applicationUser = await userManager.FindByIdAsync(model.Id);
                    applicationUser.FirstName = model.FName;
                    applicationUser.LastName = model.LName;
                    applicationUser.PhoneNumber = model.PhoneNumber;
                    await userManager.UpdateAsync(applicationUser);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);

        }

    }
}
