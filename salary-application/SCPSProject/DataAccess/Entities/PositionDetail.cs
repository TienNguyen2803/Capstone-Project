using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class PositionDetail : _BaseEntity
    {
        public string EmpCode { get; set; }
        public Employee Employee { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public DateTimeOffset? ApplyDate { get; set; }
    }
}
