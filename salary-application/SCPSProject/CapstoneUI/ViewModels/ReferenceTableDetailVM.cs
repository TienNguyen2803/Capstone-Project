using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class ReferenceTableDetailVM
  {
    public string Key { get; set; }
    public string Value { get; set; }
  }

  public class ReferenceTableDetailCreateVM
  {
    public string Key { get; set; }
    public string Value { get; set; }
    public FieldNCVM FieldVM { get; set; }
    public RefTableNCVM ReferenceVM { get; set; }
    public FormulaNCVM FormulaTypeVM { get; set; }
    public decimal Start { get; set; }
    public decimal End { get; set; }
  }
}
