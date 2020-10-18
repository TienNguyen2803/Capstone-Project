using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class ReferenceTableVM
  {
    public string Name { get; set; }
    public int SourceType { get; set; }
    public int SourceValue { get; set; }
    public string ReturnType { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public ICollection<ReferenceTableDetailVM> ReferenceTableDetailVMs { get; set; }
  }
  public class ReferenceTableElementVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
  }

  public class ReferenceTableCreateVM
  {
    public string Name { get; set; }
    public int SourceType { get; set; }
    public int SourceValue { get; set; }
    public int CompareType { get; set; }
    public string ReturnType { get; set; }
    public string Description { get; set; }
    public ICollection<ReferenceTableDetailCreateVM> ReferenceTableDetailCreateVMs { get; set; }
  }
  public class RefTableCLVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int CompareType { get; set; }
    public int SourceType { get; set; }
    public int SourceValue { get; set; }
    public string SourceName { get; set; }
    public Object FieldVM { get; set; }
    public Object RefTableVM { get; set; }
    public Object FormulaVM { get; set; }
    public ICollection<Object> RefTableDetails { get; set; }
  }
}
