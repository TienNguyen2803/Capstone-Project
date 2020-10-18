namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IReferenceTableDetailService
    {
        void Create(ReferenceTableDetail entity);
        void Update(ReferenceTableDetail entity);
        void Delete(ReferenceTableDetail entity);
        ReferenceTableDetail Get(Expression<Func<ReferenceTableDetail, bool>> predicated, params Expression<Func<ReferenceTableDetail, object>>[] includes);
        IQueryable<ReferenceTableDetail> GetAll(params Expression<Func<ReferenceTableDetail, object>>[] includes);
        void Delete(ICollection<ReferenceTableDetail> entity);
        void Create(ICollection<ReferenceTableDetail> entity);
    }
}
