using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class PayrollVM
  {
    public int Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int StandardWorkDay { get; set; }
    public decimal Revenue { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public DateTimeOffset? PayDate { get; set; }
    public PayrollStatus Status { get; set; }
    public DocumentVM DocumentVM { get; set; }
  }
  public class PayrollMailVM
  {
    public int Month { get; set; }
    public int Year { get; set; }
    public BusinessLogic.Implement.FormulaTypeSSVM formula { get; set; }
    public FormulaTypeSSVM Formula { get; set; }
  }
  public class PayrollCreateVM
  {
    public int Month { get; set; }
    public int Year { get; set; }
    public int StandardWorkDay { get; set; }
    public decimal Revenue { get; set; }
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
    public DateTimeOffset PayDate { get; set; }
    public PayrollStatus Status { get; set; }
  }

  public class PayrollGetVM
  {
    public int Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public ICollection<PayslipGetVM> Payslips { get; set; }
  }

  public class PayslipGetVM
  {
    public int Id { get; set; }
    public string EmpId { get; set; }
    public decimal Amount { get; set; }
    public EmployeeGetVM Employee { get; set; }
    public ICollection<SalaryComponentGetVM> MonthlySalaryComponents { get; set; }
  }

  public class EmployeeGetVM
  {
    public string Code { get; set; }
    public string Name { get; set; }
    public ICollection<SalaryComponentGetVM> SalaryComponents { get; set; }
  }

  [DataContract]
  public class SalaryComponentGetVM
  {
    public int FieldId { get; set; }
    [DataMember]
    public string FieldName { get; set; }
    [DataMember]
    public string Value { get; set; }
  }

}
