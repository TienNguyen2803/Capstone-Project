using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public  class ReferenceTable: _BaseEntity
    {
        public string Name { get; set; }
        public int SourceType { get; set; }
        public int SourceValue { get; set; }
        public int CompareType { get; set; }
        public string ReturnType { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public ICollection<ReferenceTableDetail> ReferenceTableDetails { get; set; }
        public ICollection<ReferenceTableType> ReferenceTableTypes { get; set; }
    }
}
