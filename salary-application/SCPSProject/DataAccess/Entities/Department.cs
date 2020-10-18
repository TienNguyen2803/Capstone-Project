using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Department : _BaseEntity
    {
        public string DepName { get; set; }
        public string DepOffice { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
