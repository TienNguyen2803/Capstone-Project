using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class FireBaseVM
  {
    public string Role { get; set; }
    public bool IsReading { get; set; }
    public int Count { get; set; }
    public string DocCode { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public NotiStatus Status { get; set; }
  }

  public enum NotiStatus
  {
    EMP_NEWPAYSLIP = 1,
    ACC_NEWPAYROLL = 2,
    ACC_DATA_MISSING = 3,
    ACC_PAYROLL_SENT = 4,
    ACC_APPLY_DOCUMENT = 5
  }
}
