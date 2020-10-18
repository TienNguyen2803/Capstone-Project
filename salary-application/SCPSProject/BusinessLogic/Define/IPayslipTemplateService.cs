namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IPayslipTemplateService
    {
        void Create(PayslipTemplate entity);
        void Update(PayslipTemplate entity);
        void Delete(PayslipTemplate entity);
        PayslipTemplate Get(Expression<Func<PayslipTemplate, bool>> predicated, params Expression<Func<PayslipTemplate, object>>[] includes);
        IQueryable<PayslipTemplate> GetAll(params Expression<Func<PayslipTemplate, object>>[] includes);
        void CreatePayslipTemplate(PayslipTemplate entity);
    }
}
