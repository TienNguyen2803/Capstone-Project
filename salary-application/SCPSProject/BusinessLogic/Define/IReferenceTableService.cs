namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IReferenceTableService
    {
        void Create(ReferenceTable entity);
        void Update(ReferenceTable entity);
        void Delete(ReferenceTable entity);
        ReferenceTable Get(Expression<Func<ReferenceTable, bool>> predicated, params Expression<Func<ReferenceTable, object>>[] includes);
        IQueryable<ReferenceTable> GetAll(params Expression<Func<ReferenceTable, object>>[] includes);
        void Create(ReferenceTable entity, ICollection<ReferenceTableDetail> referenceTableDetails);
    }
}
