using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IConstantTypeService
    {
        void Create(ConstantType entity);
        void Update(ConstantType entity);
        void Delete(ConstantType entity);
        ConstantType Get(Expression<Func<ConstantType, bool>> predicated, params Expression<Func<ConstantType, object>>[] includes);
        IQueryable<ConstantType> GetAll(params Expression<Func<ConstantType, object>>[] includes);
    }
}
