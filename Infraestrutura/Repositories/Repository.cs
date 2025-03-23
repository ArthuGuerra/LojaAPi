using Domain.Interfaces;
using System.Linq.Expressions;
using Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infraestrutura.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private LojinhaContext _context;
        public Repository(LojinhaContext context) 
        {
            _context = context;
        }
        public T Create(T entity)
        {
            System.Threading.Thread.Sleep(3000);
            _context.Add(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {

            System.Threading.Thread.Sleep(3000);
            var colection = await _context.Set<T>().AsNoTracking().ToListAsync();
            return colection;
           
        }

        public async Task<T> GetById(Expression<Func<T,bool>> predicate)
        {
            System.Threading.Thread.Sleep(3000);
            var aux = await _context.Set<T>().FirstOrDefaultAsync(predicate);
            return aux;
        }

        public T Update(Guid id,T entity)
        {         

            try
            {
                var aux = _context.Set<T>().Find(id);

                if (aux is null)
                {
                    throw new Exception("Ocorreu uma exceção. Id nao encontrado");
                }
                else
                {
                    _context.Set<T>().Entry(aux).State = EntityState.Modified;
                    return aux;
                }             

            }
            catch(Exception ex)
            {
                throw new Exception("Ocorreu uma exceção ao Salvar o UpDate " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
           
        }

        public T Delete(Guid id)
        {
            try
            {
                var aux = _context.Set<T>().Find(id);

                if(aux is null)
                {
                    throw new Exception("Ocorrue uma exceção ao excluir. Id nao encontrado");
                }
                else
                {
                    _context.Set<T>().Remove(aux);

                    return aux;
                }
            

            } catch(Exception ex)
            {
                throw new Exception("Ocorrue uma exceção ao excluir." + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }
    }
}
