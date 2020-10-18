using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class MonthlySalaryComponent : _BaseEntity
    {
        public int PayslipId { get; set; }
        public string Value { get; set; }
        public int FieldId { get; set; }
        public Payslip Payslip { get; set; }
        public Field Field { get; set; }
    }
}
