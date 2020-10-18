using BusinessLogic.Define;
using CapstoneUI.Utils;
using CapstoneUI.ViewModels;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : _BaseController
  {
    private readonly IAccountService _accountService;
    private readonly IEmployeeService _employeeService;
    private readonly IRoleService _roleService;
    private AccountKhoiNKT user = new AccountKhoiNKT();

    public AccountController(IAccountService accountService,
      IRoleService roleService, IEmployeeService employeeService)
    {
      _accountService = accountService;
      _roleService = roleService;
      _employeeService = employeeService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      var acc = _accountService.Get(a => a.Code == model.code && a.Password == model.password, _ => _.Role);
      var emp = _employeeService.Get(a => a.Code == model.code);


      if (acc == null)
        return Unauthorized();

      #region KhoiNKT
      user.Code = emp.Code;
      user.Name = emp.Fullname;
      user.RoleName = acc.Role.Name;
      user.Email = emp.Email;
      #endregion

      var identity = await GetIdentityAsync(acc);

      var principal = new ClaimsPrincipal(identity);
      var utcNow = DateTime.UtcNow;
      var props = new AuthenticationProperties()
      {
        IssuedUtc = utcNow,
        ExpiresUtc = utcNow.AddHours(1000000)
      };
      var ticket = new AuthenticationTicket(principal, props,
          principal.Identity.AuthenticationType);

      var resp = GenerateTokenResponse(ticket);
      return Ok(resp);
    }


    private async Task<ClaimsIdentity> GetIdentityAsync(Account acc)
    {
      var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
      identity.AddClaim(new Claim(ClaimTypes.Name, acc.Code));
      identity.AddClaim(new Claim(ClaimTypes.Role, acc.Role.Name));
      return identity;
    }

    private TokenResponseViewModel GenerateTokenResponse(AuthenticationTicket ticket)
    {
      #region Generate JWT Token
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.Default.GetBytes("thisismysecretkey123123123");
      var issuer = "spcs";
      var audience = "spcs";
      var identity = ticket.Principal.Identity as ClaimsIdentity;
      identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
      identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, ticket.Principal.Identity.Name));

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Issuer = issuer,
        Audience = audience,
        Subject = identity,
        IssuedAt = ticket.Properties.IssuedUtc?.UtcDateTime,
        Expires = ticket.Properties.ExpiresUtc?.UtcDateTime,
        SigningCredentials = new SigningCredentials(
              new SymmetricSecurityKey(key),
              SecurityAlgorithms.HmacSha256Signature),
        NotBefore = ticket.Properties.IssuedUtc?.UtcDateTime
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);
      #endregion

      var resp = new TokenResponseViewModel();
      resp.access_token = tokenString;
      resp.user = user;
      return resp;
    }

  }

}
