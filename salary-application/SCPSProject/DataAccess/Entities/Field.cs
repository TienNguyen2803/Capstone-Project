using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Entities
{
    public class Field : _BaseEntity
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public string DataType { get; set; }
        public bool Status { get; set; }
        public string CellMapping { get; set; }
        public bool IsMonthlyComponent { get; set; }
        public string Description { get; set; }
        public ICollection<MonthlySalaryComponent> MonthlySalaryComponents { get; set; }
        public ICollection<PayrollComponent> PayrollComponents { get; set; }
        public ICollection<SalaryComponent> SalaryComponents { get; set; }
        public ICollection<FieldType> FieldTypes { get; set; }
        public string SampleValue { get; set; }

        [NotMapped]
        public string Value { get; set; }
    }
}
