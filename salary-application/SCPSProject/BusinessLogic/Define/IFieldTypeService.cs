namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFieldTypeService
    {
        void Create(FieldType entity);
        void Update(FieldType entity);
        void Delete(FieldType entity);
        FieldType Get(Expression<Func<FieldType, bool>> predicated, params Expression<Func<FieldType, object>>[] includes);
        IQueryable<FieldType> GetAll(params Expression<Func<FieldType, object>>[] includes);
        void Delete(ICollection<FieldType> entity);
    }
}
