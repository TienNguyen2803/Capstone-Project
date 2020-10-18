namespace DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T> where T : class
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Unchange(T entity);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        
        T Get(Expression<Func<T, bool>> predicated, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> Search(Func<T, bool> predicated, params Expression<Func<T, object>>[] includes);

        IEnumerable<TSelector> Select<TSelector>(Func<T, TSelector> selector,
            params Expression<Func<T, object>>[] includes);
    }
}
