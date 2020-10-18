using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class PositionDetailVM
  {
  }

  public class PositionDetailCreateVM
  {
    public int PositionId { get; set; }
    public DateTimeOffset ApplyDate { get; set; }
  }
}
