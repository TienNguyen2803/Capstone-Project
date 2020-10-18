using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class ConstantType
    {
        public int FormulaDetailId { get; set; }
        public decimal Value { get; set; }
        public FormulaDetail FormulaDetail { get; set; }
    }
}
