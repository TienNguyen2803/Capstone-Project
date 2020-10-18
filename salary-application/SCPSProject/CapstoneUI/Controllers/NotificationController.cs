using CapstoneUI.Firebase;
using CapstoneUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  public class NotificationController : _BaseController
  {
    [HttpPost]
    public async Task<IActionResult> Post(FireBaseVM viewModel)
    {
      try
      {
        await FirebaseDatabase.GetFirebaseDatabase().sendMessage(viewModel);
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }
  }
}
