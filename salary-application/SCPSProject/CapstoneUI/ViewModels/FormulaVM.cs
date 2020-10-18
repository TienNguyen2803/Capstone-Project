using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class FormulaVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string Formula { get; set; }
    public ICollection<FormulaDetailVM> FormulaDetails { get; set; }
  }

  public class FormulaShowVM
  {
    public string Name { get; set; }
    public string Expression { get; set; }

    public override bool Equals(object obj)
    {
      return obj is FormulaShowVM vM &&
             Name == vM.Name;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Name);
    }
  }
  public class FormulaDocVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
  }
  public class FormulaReturnVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
  public class FormulaElementVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
  }

  public class FormulaCreateVM
  {
    public string Name { get; set; }
    public int Type { get; set; }
    public bool IsSalaryFormula { get; set; }
    public string Description { get; set; }
    public int DocId { get; set; }

    public ICollection<FormulaDetailCreateVM> FormulaDetailCreateVMs { get; set; }
  }

  public class FormulaTypeSSVM
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int FormulaType { get; set; }
    public ICollection<FormulaDetailSSVM> Details { get; set; }
    public decimal Value { get; set; }
    public string Formula { get; set; }
  }

  public class FormulaDetailSSVM
  {
    public int Id { get; set; }
    public int Type { get; set; }
    public int Ordinal { get; set; }
    public int Operator { get; set; }
    public ConstantTypeSSVM ConstantType { get; set; }
    public RefTypeSSVM RefType { get; set; }
    public RefTypeSSVM FieldType { get; set; }
    public FormulaTypeSSVM FormulaType { get; set; }
    public string Value { get; set; }
  }

  public class ConstantTypeSSVM
  {
    public decimal Value { get; set; }
  }

  public class RefTypeSSVM
  {
    public string Name { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
  }

  public class FieldTypeSSVm
  {
    public string Name { get; set; }
    public string Value { get; set; }
  }

  public class FormulaNCVM
  {

    public string Name { get; set; }
    public int FormulaType { get; set; }
    public bool IsSalaryFormula { get; set; }
    public string Description { get; set; }
    public int DocId { get; set; }
    public ICollection<ICollection<FieldCVM>> FieldsandValuesTest { get; set; }
    public ICollection<FormulaDetailNCVM> FormulaDetailNCVMs { get; set; }
  }

  public class FormulaElementNCVM
  {
    public string Name { get; set; }
    public string Value { get; set; }
    public string Expression { get; set; }
    public int Type { get; set; }

    public override bool Equals(object obj)
    {
      return obj is FormulaElementNCVM nCVM &&
             Name == nCVM.Name &&
             Value == nCVM.Value &&
             Type == nCVM.Type;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Name, Value, Type);
    }
  }
  public class FormulaDetailNCVM
  {
    public int Operator { get; set; }
    public int Type { get; set; }
    public string Name { get; set; }
    public FDTypeNCVM FDTypeVM { get; set; }
    public FieldNCVM FieldTypeVM { get; set; }
    public RefTableNCVM RefTableTypeVM { get; set; }
    public FormulaNCVM FormulaTypeVM { get; set; }
    public ConstantTypeVM ConstantTypeVM { get; set; }
  }

  public class FDTypeNCVM
  {
    public int Id { get; set; }
    public decimal Value { get; set; }
  }
  public class FieldNCVM
  {
    public string Name { get; set; }
    public string LongName { get; set; }
    public string DataType { get; set; }
    public bool IsMonthlySalaryComponent { get; set; }
    public string Description { get; set; }
  }

  public class RefTableNCVM
  {
    public string Name { get; set; }
    public int SourceType { get; set; }
    public string SourceName { get; set; }
    public string ReturnType { get; set; }
    public int CompareType { get; set; }
    public string Description { get; set; }
    public FieldNCVM FieldVM { get; set; }
    public RefTableNCVM ReferenceVM { get; set; }
    public FormulaNCVM FormulaTypeVM { get; set; }
    public ICollection<ReferenceTableDetailCreateVM> ReferenceTableDetailCreateVMs { get; set; }
  }

}
