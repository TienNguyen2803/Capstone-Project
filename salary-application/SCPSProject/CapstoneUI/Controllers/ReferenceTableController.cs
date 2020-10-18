using BusinessLogic.Define;
using CapstoneUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class ReferenceTableController : _BaseController
  {
    private readonly IReferenceTableService _referenceTableService;


    public ReferenceTableController(IReferenceTableService referenceTableService)
    {
      _referenceTableService = referenceTableService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] ReferenceTableCreateVM viewModel)
    {
      try
      {
        _referenceTableService.Create(ModelMapper.ConvertToModel(viewModel), ModelMapper.ConvertToModel(viewModel.ReferenceTableDetailCreateVMs));
        return Ok(ModelMapper.ConvertToViewModel(_referenceTableService.Get(_ => _.Name == viewModel.Name, _ => _.ReferenceTableDetails)));
      }
      catch (Exception e)
      {
        return StatusCode(500,e);
      }

    }

    [HttpGet]
    [Route("referencetables")]
    public IActionResult Get()
    {
      try
      {
        //var result = _referenceTableService.Get(x => true, x => x.ReferenceTableDetails);
        var result = _referenceTableService.GetAll(_ => _.ReferenceTableDetails).ToList();
        return Ok(ModelMapper.ConvertToViewModel(result));
      }
      catch (Exception e)
      {
        return StatusCode(500);
      }

    }
  }
}
