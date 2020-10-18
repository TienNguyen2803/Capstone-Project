namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IPayrollComponentService
    {
        void Create(PayrollComponent entity);
        void Update(PayrollComponent entity);
        void Delete(PayrollComponent entity);
        PayrollComponent Get(Expression<Func<PayrollComponent, bool>> predicated, params Expression<Func<PayrollComponent, object>>[] includes);
        IQueryable<PayrollComponent> GetAll(params Expression<Func<PayrollComponent, object>>[] includes);
        void Create(ICollection<PayrollComponent> entity);
        void Delete(ICollection<PayrollComponent> enity);
        void Update(ICollection<PayrollComponent> enity);
    }
}
