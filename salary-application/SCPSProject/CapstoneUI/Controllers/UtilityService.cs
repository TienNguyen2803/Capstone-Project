using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  public class UtilityService
  {
    public static string CommaForBigNum(string num)
    {
      // check field 
      if (!Int32.TryParse(num, out int temp))
      {
        // with string return
        return num;
      }
      else
      {
        // with int return
        // process
        string result = "";
        int i = num.Length - 3;
        while (true)
        {
          if (i < 1) break;
          result = "." + num.Substring(i, 3) + result;
          num = num.Substring(0, i);
          i -= 3;
        }
        result = num + result;
        return result;

      }
    }
  }
}
