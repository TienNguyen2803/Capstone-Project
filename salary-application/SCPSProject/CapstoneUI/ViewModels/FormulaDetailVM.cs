using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class FormulaDetailVM
  {
    public int Id { get; set; }
    public int Operator { get; set; }
    public int Type { get; set; }
    public FieldTypeVM FieldTypeVM { get; set; }
    public RefTableTypeVM RefTableTypeVM { get; set; }
    public FormulaVM FormulaTypeVM { get; set; }
    public ConstantTypeVM ConstantTypeVM { get; set; }

  }


  public class FormulaDetailGetVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
  }

  public class FormulaDetailTypeCreateVM
  {
    public int Id { get; set; }
    public decimal Value { get; set; }
    
  }

  public class FormulaDetailCreateVM
  {
    public int Type { get; set; }
    public int Ordinal { get; set; }
    public int Operator { get; set; }
    public FormulaDetailTypeCreateVM FDType { get; set; }
  }
}
