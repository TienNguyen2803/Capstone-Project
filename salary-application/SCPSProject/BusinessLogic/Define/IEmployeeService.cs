namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IEmployeeService
    {
        void Create(Employee entity);
        void Update(Employee entity);
        void Delete(Employee entity);
        Employee Get(Expression<Func<Employee, bool>> predicated, params Expression<Func<Employee, object>>[] includes);
        IQueryable<Employee> GetAll(params Expression<Func<Employee, object>>[] includes);

        void Create(Employee entity, PositionDetail positionDetail);
    }
}
