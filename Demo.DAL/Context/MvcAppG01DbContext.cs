using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Context
{
    public class MvcAppG01DbContext : IdentityDbContext<ApplicationUser>
    {
        public MvcAppG01DbContext(DbContextOptions<MvcAppG01DbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
