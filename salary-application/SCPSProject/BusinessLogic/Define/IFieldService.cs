namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFieldService
    {
        void Create(Field entity);
        void Update(Field entity);
        void Delete(Field entity);
        Field Get(Expression<Func<Field, bool>> predicated, params Expression<Func<Field, object>>[] includes);
        IQueryable<Field> GetAll(params Expression<Func<Field, object>>[] includes);

        Field GetField(int fieldId, int payslipId, string employeeId);
    }
}
