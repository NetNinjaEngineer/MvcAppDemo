using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
