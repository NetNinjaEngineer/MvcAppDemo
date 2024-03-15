using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.BL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
