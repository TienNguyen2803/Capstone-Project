namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFormulaService
    {
        void Create(Formula entity);
        void Update(Formula entity);
        void Delete(Formula entity);
        Formula Get(Expression<Func<Formula, bool>> predicated, params Expression<Func<Formula, object>>[] includes);
        IQueryable<Formula> GetAll(params Expression<Func<Formula, object>>[] includes);
        void Create(Formula entity, ICollection<FormulaDetail> formulaDetails, int docId);
    }
}
