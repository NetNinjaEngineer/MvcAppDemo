using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Context
{
    public class MvcAppG01DbContext : DbContext
    {
        public MvcAppG01DbContext(DbContextOptions<MvcAppG01DbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
    }
}
