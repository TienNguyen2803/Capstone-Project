namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IMonthlySalaryComponentService
    {
        void Create(MonthlySalaryComponent entity);
        void Update(MonthlySalaryComponent entity);
        void Delete(MonthlySalaryComponent entity);
        MonthlySalaryComponent Get(Expression<Func<MonthlySalaryComponent, bool>> predicated, params Expression<Func<MonthlySalaryComponent, object>>[] includes);
        IQueryable<MonthlySalaryComponent> GetAll(params Expression<Func<MonthlySalaryComponent, object>>[] includes);
        void Create(ICollection<MonthlySalaryComponent> entity);
        void Delete(ICollection<MonthlySalaryComponent> enity);
        void Update(ICollection<MonthlySalaryComponent> enity);
    }
}
