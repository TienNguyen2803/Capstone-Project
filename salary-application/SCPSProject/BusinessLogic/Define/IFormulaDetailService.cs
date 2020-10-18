namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFormulaDetailService
    {
        void Create(FormulaDetail entity);
        void Update(FormulaDetail entity);
        void Delete(FormulaDetail entity);
        FormulaDetail Get(Expression<Func<FormulaDetail, bool>> predicated, params Expression<Func<FormulaDetail, object>>[] includes);
        IQueryable<FormulaDetail> GetAll(params Expression<Func<FormulaDetail, object>>[] includes);

        void CreateType(int type, int formularDetailId, int refId, decimal value);
        void Delete(ICollection<FormulaDetail> formulaDetails);
    }
}
