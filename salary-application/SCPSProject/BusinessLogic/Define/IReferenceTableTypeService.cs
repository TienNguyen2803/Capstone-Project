namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IReferenceTableTypeService
    {
        void Create(ReferenceTableType entity);
        void Update(ReferenceTableType entity);
        void Delete(ReferenceTableType entity);
        ReferenceTableType Get(Expression<Func<ReferenceTableType, bool>> predicated, params Expression<Func<ReferenceTableType, object>>[] includes);
        IQueryable<ReferenceTableType> GetAll(params Expression<Func<ReferenceTableType, object>>[] includes);
        void Delete(ICollection<ReferenceTableType> entity);
    }
}
