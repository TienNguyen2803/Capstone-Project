using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Formula : _BaseEntity
    {
        public string Name { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public bool IsSalaryFormula { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public ICollection<FormulaDetail> FormulaDetails { get; set; }
        public Document Document { get; set; }
        public ICollection<FormulaType> FormulaTypes { get; set; }
    }
}
