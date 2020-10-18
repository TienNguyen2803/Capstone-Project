using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class FieldType
    {
        public int FormulaDetailId { get; set; }
        public int FieldId { get; set; }
        public FormulaDetail FormulaDetail { get; set; }
        public Field Field { get; set; }
    }
}
