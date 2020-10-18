using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Account
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Employee Employee { get; set; }
    }
}
