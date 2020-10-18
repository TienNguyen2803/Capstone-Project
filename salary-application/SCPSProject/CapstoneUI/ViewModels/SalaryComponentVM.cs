using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class SalaryComponentVM
  {
    public string EmpId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; }
    public DateTimeOffset ApplyDate { get; set; }
  }
  public class SalaryComponentGAVM
  {
    public int docId { get; set; }
    public ICollection<FieldElementVM> salaryFields { get; set; }
    public ICollection<EmployeeGAVM> emps { get; set; }
  }
  public class SalaryComponentCreateVM
  {
    public string EmpId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; }
  }
  public class MonthlySalaryComponentVM
  {
    public string EmpId { get; set; }
    public int PayrollId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; }
  }
}
