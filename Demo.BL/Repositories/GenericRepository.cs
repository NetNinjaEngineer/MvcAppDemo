using Demo.BL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MvcAppG01DbContext _context;

        public GenericRepository(MvcAppG01DbContext context)
            => _context = context;

        public async Task Create(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<T> Get(int id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>)await _context.Employees.Include(e => e.Department).ToListAsync();
            return await _context.Set<T>().ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
