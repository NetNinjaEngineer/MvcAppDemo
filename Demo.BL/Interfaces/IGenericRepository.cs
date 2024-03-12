using System.Collections.Generic;

namespace Demo.BL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        int Create(T entity);
        int Update(T entity);
        int Delete(T entity);
    }
}
