using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class PayslipVM
  {
  }
  public class PaySlipMailVM
  {
    public string EmpName { get; set; }
    public string Department { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public PayrollMailVM payrollVM { get; set; }

  }
  public class CheckPayslipVM
  {
    public int Month { get; set; }
    public int Year { get; set; }
    public ICollection<string> Employees { get; set; }

  }
}
