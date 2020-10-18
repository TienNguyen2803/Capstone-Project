using BusinessLogic.Define;
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
  public class FormulaDetailController : _BaseController
  {
    private readonly IFieldService _fieldService;
    private readonly IFormulaService _formulaService;
    private readonly IReferenceTableService _referenceTableService;
    private readonly IFormulaDetailService _formulaDetailService;
    public FormulaDetailController(IFieldService fieldService, IFormulaService formulaService, IReferenceTableService referenceTableService, IFormulaDetailService formulaDetailService)
    {
      _fieldService = fieldService;
      _formulaService = formulaService;
      _referenceTableService = referenceTableService;
      _formulaDetailService = formulaDetailService;
    }

    [HttpGet]
    [Route("formula-create-elements")]
    public IActionResult Get()
    {
      try
      {
        var listField = ModelMapper.ConvertToElementViewModel(_fieldService.GetAll().Where(_ => _.Name != "Dư nợ").ToList()).ToList();
        var listReferenceTable = ModelMapper.ConvertToElementViewModel(_referenceTableService.GetAll().ToList()).ToList();
        var lisFormula =ModelMapper.ConvertToElementViewModel(_formulaService.GetAll().Where(_ => _.IsSalaryFormula == false).ToList()).ToList();

        listField.AddRange(listReferenceTable);
        listField.AddRange(lisFormula);
        var result = listField;
        return Ok(result);
      }
      catch (Exception e)
      {
        return StatusCode(500);
      }

    }

    [HttpGet]
    [Route("formula-details")]
    public IActionResult GetAllFormulaDetailOfFormula(int formulaId)
    {
      try
      {
        var result = _formulaDetailService.GetAll(_ => _.Formula)
                    .Where(_ => _.FormulaId == formulaId).ToList();
        return Ok(ModelMapper.ConvertToViewModel(result));
      }
      catch (Exception e)
      {
        return StatusCode(500,e);
      }

    }
  }
}
