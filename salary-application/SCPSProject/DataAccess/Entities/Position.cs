using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Position : _BaseEntity
    {
        public string Name { get; set; }
        public ICollection<PositionDetail> PositionDetails { get; set; }
    }
}
