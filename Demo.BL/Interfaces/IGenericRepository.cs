using System.Collections.Generic;

namespace Demo.BL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
