using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class FieldVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string LongName { get; set; }
    public string DataType { get; set; }
    public bool Status { get; set; }
    public string CellMapping { get; set; }
    public bool IsMonthlyComponent { get; set; }
    public string Description { get; set; }
  }
  public class FiellExelVM
  {
    public string Field { get; set; }
  }
  public class FieldElementVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
  }

  public class FieldCreateVM
  {
    public string Name { get; set; }
    public string LongName { get; set; }
    public string DataType { get; set; }
    public bool IsMonthlySalaryComponent { get; set; }
    public string Description { get; set; }
    public string SampleValue { get; set; }
  }

  public class FieldCVM
  {
    public string Name { get; set; }
    public string Value { get; set; }

    public override bool Equals(object obj)
    {
      return obj is FieldCVM cVM &&
             Name == cVM.Name;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Name);
    }
  }

}
