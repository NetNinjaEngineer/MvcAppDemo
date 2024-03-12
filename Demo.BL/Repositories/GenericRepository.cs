using Demo.BL.Interfaces;
using Demo.DAL.Context;
using System.Collections.Generic;
using System.Linq;

namespace Demo.BL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MvcAppG01DbContext _context;

        public GenericRepository(MvcAppG01DbContext context)
            => _context = context;

        public int Create(T entity)
        {
            _context.Add(entity);
            return _context.SaveChanges();
        }

        public int Delete(T entity)
        {
            _context.Remove(entity);
            return _context.SaveChanges();
        }

        public T Get(int id)
            => _context.Set<T>().Find(id);

        public IEnumerable<T> GetAll()
            => _context.Set<T>().ToList();

        public int Update(T entity)
        {
            _context.Update(entity);
            return _context.SaveChanges();
        }
    }
}
