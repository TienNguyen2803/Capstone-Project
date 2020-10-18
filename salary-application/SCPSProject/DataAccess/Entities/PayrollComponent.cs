using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class PayrollComponent : _BaseEntity
    {
        public int PayrollId { get; set; }
        public string Value { get; set; }
        public int FieldId { get; set; }
        public Payroll Payroll { get; set; }
        public Field Field { get; set; }
    }
}
