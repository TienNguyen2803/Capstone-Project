using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IDepartmentService
    {
        void Create(Department entity);
        void Update(Department entity);
        void Delete(Department entity);
        Department Get(Expression<Func<Department, bool>> predicated, params Expression<Func<Department, object>>[] includes);
        IQueryable<Department> GetAll(params Expression<Func<Department, object>>[] includes);
    }
}
