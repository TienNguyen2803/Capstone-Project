using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Document : _BaseEntity
    {
        public string Code { get; set; }
        public DateTimeOffset? SignDate { get; set; }
        public DateTimeOffset? ApplyDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int CloseDay { get; set; }
        public int Deadline { get; set; }
        public string Description { get; set; }
        public int? FormulaId { get; set; }
        public Formula Formula { get; set; }
        public DocStatus Status { get; set; }
        public string DocumentUrl { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }
        public ICollection<PayslipTemplate> PayslipTemplates { get; set; }
    }

    public enum DocStatus
    {
        Deactive = 1,
        Active = 2,
        Priority = 3
    }
}
