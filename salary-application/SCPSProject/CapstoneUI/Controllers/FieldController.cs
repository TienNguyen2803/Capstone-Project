using BusinessLogic.Define;
using CapstoneUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class FieldController : _BaseController
  {
    private readonly IFieldService _fieldService;
    private readonly IDocumentService _documentService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IFormulaService _formulaService;
    public FieldController(IFieldService fieldService, IDocumentService documentService,
      IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService,
      IFormulaTypeService formulaTypeService, IFormulaService formulaService)
    {
      _fieldService = fieldService;
      _documentService = documentService;
      _fieldTypeService = fieldTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _formulaTypeService = formulaTypeService;
      _formulaService = formulaService;
    }

    public FieldController(IFieldService fieldService)
    {
      _fieldService = fieldService;
    }

    [Route("field")]
    [HttpPost]
    public IActionResult Create([FromBody] FieldCreateVM viewModel)
    {
      try
      {
        _fieldService.Create(ModelMapper.ConvertToModel(viewModel));

        return Ok(ModelMapper.ConvertToViewModel(
          _fieldService.Get(_ => _.Name.Equals(viewModel.Name))));
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }

    [Route("fields")]
    [HttpGet]
    public IActionResult GetAll()
    {
      try
      {
        return Ok(ModelMapper.ConvertToViewModel(_fieldService.GetAll().ToList()));
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }


    [Route("fields-by-doc")]
    [HttpGet]
    public IActionResult GetField(bool isMonthly)
    {
      try
      {
        return Ok(GetFieldByDoc(isMonthly).ToList());
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    //isMonthly=true: get field monthly
    //isMonthly=false: get field Salary
    private ICollection<Field> GetFieldByDoc(bool isMonthly)
    {
      var document = _documentService.GetAll(_ => _.Formula)
        .Where(_ => _.ApplyDate <= DateTimeOffset.Now && _.FormulaId != null)
        .OrderByDescending(_ => _.ApplyDate).FirstOrDefault();
      var formula = document.Formula;
      ICollection<Field> resultS = new HashSet<Field>();
      ICollection<Field> resultM = new HashSet<Field>();
      var listField = GetFieldByFormula(formula);
      if (listField.Count != 0)
      {
        foreach (var field in listField)
        {
          if (field.IsMonthlyComponent)
          {
            resultM.Add(field);
          }
          else
          {
            if (field.Name != "Chức vụ")
            {
              resultS.Add(field);
            }
          }
        }
      }
      else return null;
      if (isMonthly) return resultM;
      else return resultS;
    }

    //recursive func to get field from formula 
    private ICollection<Field> GetFieldByFormula(Formula form)
    {
      ICollection<Field> list = new HashSet<Field>();
      var formula = _formulaService.Get(_ => _.Id == form.Id, _ => _.FormulaDetails);
      formula.FormulaDetails.ToList().ForEach(fDetail =>
      {
        switch (fDetail.Type)
        {
          case 1:
            var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
            var field = _fieldService.Get(_ => _.Id == fieldType.FieldId);
            list.Add(field);
            break;
          case 2:
            var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.ReferenceTable.ReferenceTableDetails);
            var field2 = _fieldService.Get(_ => _.Id == refType.ReferenceTable.SourceValue);
            list.Add(field2);
            break;
          case 3:
            var formular = _formulaTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.Formula.FormulaDetails);
            var subList = GetFieldByFormula(formular.Formula);
            list = list.Union(subList).ToList();
            break;
        }
      });
      return list;
    }
  }
}
