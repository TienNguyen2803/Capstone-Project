namespace BusinessLogic.Define
{
    using BusinessLogic.Implement;
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IPayslipService
    {
        void Create(Payslip entity);
        void Update(Payslip entity);
        void Delete(Payslip entity);
        Payslip Get(Expression<Func<Payslip, bool>> predicated, params Expression<Func<Payslip, object>>[] includes);
        IQueryable<Payslip> GetAll(params Expression<Func<Payslip, object>>[] includes);

        void Create(int payrollId, string employeeId);
        FormulaTypeSSVM GetFormula(Formula formula, int payslipId, string employeeId);
        FormulaTypeSSVM GetFormulaV2(Formula formula, int payslipId, string employeeId);
        void Delete(ICollection<Payslip> entity);
    }
}
