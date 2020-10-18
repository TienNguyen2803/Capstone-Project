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
    public class FieldService : _BaseService<Field>, IFieldService
    {
        private readonly IEmployeeService _employeeService;
        public FieldService(IUnitOfWork unitOfWork, IEmployeeService employeeService) : base(unitOfWork)
        {
            _employeeService = employeeService;
        }

        public Field GetField(int fieldId, int payslipId, string employeeId)
        {
            string value = "";
            var field = this.Get(_ => _.Id == fieldId, _ => _.SalaryComponents, _ => _.MonthlySalaryComponents, _ => _.PayrollComponents);
            var payrollId = _employeeService.Get(_ => _.Code == employeeId, _ => _.Payslips).Payslips.FirstOrDefault(__ => __.Id == payslipId).PayrollId;
            try
            {
                if (field.DataType == "payroll")
                {
                    value = field.PayrollComponents.FirstOrDefault(z => z.PayrollId == payrollId).Value;
                }
                else if (field.IsMonthlyComponent)
                {
                    value = field.MonthlySalaryComponents.FirstOrDefault(z => z.PayslipId == payslipId).Value;
                }
                else
                {
                    var applyday = DateTimeOffset.Now;
                    var list = field.SalaryComponents.Where(_ => _.ApplyDate.Value.ToUnixTimeSeconds() <= applyday.ToUnixTimeSeconds()
                                                                && _.EmpId == employeeId).ToList();
                    value = list.OrderByDescending(_ => _.ApplyDate).FirstOrDefault().Value;
                }
            }
            catch (NullReferenceException e)
            {
                value = "N/A";
            }

            field.Value = value;

            return field;
        }
    }
}
