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

    public interface IPositionDetailService
    {
        void Create(PositionDetail entity);
        void Update(PositionDetail entity);
        void Delete(PositionDetail entity);
        PositionDetail Get(Expression<Func<PositionDetail, bool>> predicated, params Expression<Func<PositionDetail, object>>[] includes);
        IQueryable<PositionDetail> GetAll(params Expression<Func<PositionDetail, object>>[] includes);
    }
}
