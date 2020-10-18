using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
  public  class Role : _BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
