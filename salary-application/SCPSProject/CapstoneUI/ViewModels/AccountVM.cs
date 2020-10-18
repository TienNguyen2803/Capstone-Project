using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace CapstoneUI.ViewModels
{
  public class AccountVM
  {
  }

  public class AccountCreateVM
  {

  }

  public class AccountKhoiNKT
  {
    public AccountKhoiNKT()
    {
    }

    public string Code { get; set; }

    public string Name { get; set; }

    public string RoleName { get; set; }

    public string Email { get; set; }
  }

  public class TokenResponseViewModel
  {
    [JsonProperty("access_token")]
    public string access_token { get; set; }

    [JsonProperty("user")]
    public AccountKhoiNKT user { get; set; }
  }

  public class LoginViewModel
  {
    public string code { get; set; }
    public string password { get; set; }
  }
}
