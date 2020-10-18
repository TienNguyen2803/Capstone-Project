using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Implement
{
    public class PayrollService : _BaseService<Payroll>, IPayrollService
    {
        private readonly IPayslipService _payslipService;
        private readonly IEmployeeService _employeeService;
        public PayrollService(IUnitOfWork unitOfWork, IEmployeeService employeeService,
            IPayslipService payslipService) : base(unitOfWork)
        {
            _payslipService = payslipService;
            _employeeService = employeeService;
        }

        public void CreatePayroll(Payroll entity, Document document)
        {
            //document will be applied to payroll
            entity.DocId = document.Id;

            this.Create(entity);
            foreach (var emp in _employeeService.GetAll().Where(_ => _.IsWorking == true).ToList())
            {
                _payslipService.Create(entity.Id, emp.Code);    
            }
        }
    }
}
