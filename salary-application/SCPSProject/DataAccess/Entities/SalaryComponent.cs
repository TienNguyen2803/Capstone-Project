using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
public   class SalaryComponent: _BaseEntity
    {
        public string Value { get; set; }
        public string EmpId { get; set; }
        public int FieldId { get; set; }
        public DateTimeOffset? ApplyDate { get; set; }
        public Employee Employee { get; set; }
        public Field Field { get; set; }
    }
}
