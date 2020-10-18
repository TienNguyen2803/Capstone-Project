namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFormulaTypeService
    {
        void Create(FormulaType entity);
        void Update(FormulaType entity);
        void Delete(FormulaType entity);
        FormulaType Get(Expression<Func<FormulaType, bool>> predicated, params Expression<Func<FormulaType, object>>[] includes);
        IQueryable<FormulaType> GetAll(params Expression<Func<FormulaType, object>>[] includes);
        void Delete(ICollection<FormulaType> entity);
    }
}
