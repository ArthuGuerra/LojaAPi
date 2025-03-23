using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Expression<Func<T,bool>> predicate);
        T Create(T entity);
        T Update(Guid id,T entity);
        T Delete(Guid id);
    }
}
