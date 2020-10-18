namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRoleService
    {
        void Create(Role entity);
        void Update(Role entity);
        void Delete(Role entity);
        Role Get(Expression<Func<Role, bool>> predicated, params Expression<Func<Role, object>>[] includes);
        IQueryable<Role> GetAll(params Expression<Func<Role, object>>[] includes);
    }
}
