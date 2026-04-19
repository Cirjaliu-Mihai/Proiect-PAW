using System.Linq.Expressions;
using BarberSalon.Data;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected BarberSalonContext BarberContext { get; set; }

        public RepositoryBase(BarberSalonContext barberContext)
        {
            BarberContext = barberContext;
        }

        public IQueryable<T> FindAll()
        {
            return BarberContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return BarberContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            BarberContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            BarberContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            BarberContext.Set<T>().Remove(entity);
        }
    }
}