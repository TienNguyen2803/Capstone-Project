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

    public interface IPositionService
    {
        void Create(Position entity);
        void Update(Position entity);
        void Delete(Position entity);
        Position Get(Expression<Func<Position, bool>> predicated, params Expression<Func<Position, object>>[] includes);
        IQueryable<Position> GetAll(params Expression<Func<Position, object>>[] includes);
    }
}
