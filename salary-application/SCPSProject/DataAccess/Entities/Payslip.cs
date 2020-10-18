using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Payslip : _BaseEntity
    {
        public string EmpId { get; set; }
        public int PayrollId { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public Employee Employee { get; set; }
        public Payroll Payroll { get; set; }
        public ICollection<MonthlySalaryComponent> MonthlySalaryComponents { get; set; }
    }
}
