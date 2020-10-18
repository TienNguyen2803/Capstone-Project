using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Entities
{
    public class FormulaDetail : _BaseEntity
    {
        public int Type { get; set; }
        public int FormulaId { get; set; }
        public int Ordinal { get; set; }
        public int Operator { get; set; }
        public Formula Formula { get; set; }
        public FormulaType FormulaType { get; set; }
        public FieldType FieldType { get; set; }
        public ReferenceTableType ReferenceTableType { get; set; }
        public ConstantType ConstantType { get; set; }

        [NotMapped]
        public int RefId { get; set; }

        [NotMapped]
        public decimal Value { get; set; }

        [NotMapped]
        public string Mark { get; set; }

    }
}
