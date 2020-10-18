using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class EmployeeVM
  {
  }

  public class EmployeeGAVM
  {
    public string Code { get; set; }
    public string Fullname { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public ICollection<SalaryComponentGetVM> SalaryComponents { get; set; }
  }

  public class EmployeeCreateVM
  {
    public string Code { get; set; }
    public string Fullname { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public bool IsForeigner { get; set; }
    public int DepartmentId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public PositionDetailCreateVM PositionDetailCreateVM { get; set; }
    //public AccountVM AccountCreateVM { get; set; }
  }
}
