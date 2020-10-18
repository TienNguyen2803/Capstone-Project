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

    public interface IAccountService
    {
        void Create(Account entity);
        void Update(Account entity);
        void Delete(Account entity);
        Account Get(Expression<Func<Account, bool>> predicated, params Expression<Func<Account, object>>[] includes);
        IQueryable<Account> GetAll(params Expression<Func<Account, object>>[] includes);
    }
}
