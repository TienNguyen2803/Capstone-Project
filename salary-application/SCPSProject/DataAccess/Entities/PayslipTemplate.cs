using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class PayslipTemplate : _BaseEntity
    {
        public int DocId { get; set; }
        public Document Document { get; set; }
        public string TemplateUrl { get; set; }
        public bool Status { get; set; }
    }
}
