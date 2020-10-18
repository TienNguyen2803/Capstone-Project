using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Payroll : _BaseEntity
    {
        public int DocId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int StandardWorkDay { get; set; }
        public decimal Revenue { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public DateTimeOffset? PayDate { get; set; }
        public PayrollStatus Status { get; set; }
        public Document Document { get; set; }
        public ICollection<Payslip> Payslips { get; set; }
        public ICollection<PayrollComponent> PayrollComponents { get; set; }
    }

    public enum PayrollStatus
    {
        New = 1,
        Published = 2,
        Completed = 3,
        Expried = 4
    }
}
