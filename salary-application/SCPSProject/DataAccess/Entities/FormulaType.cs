using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class FormulaType
    {
        public int FormulaDetailId { get; set; }
        public int FormulaId { get; set; }
        public FormulaDetail FormulaDetail { get; set; }
        public Formula Formula { get; set; }
    }
}
