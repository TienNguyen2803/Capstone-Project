namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface ISalaryComponentService
    {
        void Create(SalaryComponent entity);
        void Update(SalaryComponent entity);
        void Delete(SalaryComponent entity);
        SalaryComponent Get(Expression<Func<SalaryComponent, bool>> predicated, params Expression<Func<SalaryComponent, object>>[] includes);
        IQueryable<SalaryComponent> GetAll(params Expression<Func<SalaryComponent, object>>[] includes);
        void Create(ICollection<SalaryComponent> entity);
    }
}
