using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneUI.Firebase
{
  public class FirebaseMessageModel
  {
    public string userId { get; set; }
    public bool isReading { get; set; }
    public string docCode { get; set; }
    public int count { get; set; }
    public int month { get; set; }
    public int year { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public int status { get; set; }

    public FirebaseMessageModel(string userId, bool isReading, DateTimeOffset createdDate, int status, int count, int month, int year, string docCode)
    {
      this.userId = userId;
      this.isReading = isReading;
      this.CreatedDate = createdDate;
      this.status = status;
      this.docCode = docCode;
      this.count = count;
      this.month = month;
      this.year = year;
    }

  }
}
