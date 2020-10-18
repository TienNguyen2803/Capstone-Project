namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IPayrollService
    {
        void Create(Payroll entity);
        void Update(Payroll entity);
        void Delete(Payroll entity);
        Payroll Get(Expression<Func<Payroll, bool>> predicated, params Expression<Func<Payroll, object>>[] includes);
        IQueryable<Payroll> GetAll(params Expression<Func<Payroll, object>>[] includes);
        void CreatePayroll(Payroll entity, Document currentDoc);
    }
}
