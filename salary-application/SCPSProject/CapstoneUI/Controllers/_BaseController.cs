using CapstoneUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
    public class _BaseController : ControllerBase
    {
        private _ModelMapping _modelMapper;

        public _ModelMapping ModelMapper => _modelMapper ?? (_modelMapper = new _ModelMapping());
    }
}
