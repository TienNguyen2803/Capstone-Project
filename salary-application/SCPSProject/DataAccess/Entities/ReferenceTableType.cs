using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class ReferenceTableType
    {
        public int FormulaDetailId { get; set; }
        public int RefenceTableTypeId { get; set; }
        public FormulaDetail FormulaDetail { get; set; }
        public ReferenceTable ReferenceTable { get; set; }
    }
}
