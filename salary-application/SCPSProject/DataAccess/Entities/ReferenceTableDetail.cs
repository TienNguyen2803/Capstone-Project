using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Entities
{
    public class ReferenceTableDetail
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ReferenceTableId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public ReferenceTable ReferenceTable { get; set; }
    }
}
