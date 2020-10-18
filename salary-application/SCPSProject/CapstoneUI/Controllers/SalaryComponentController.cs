using BusinessLogic.Define;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
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
  public class SalaryComponentController : _BaseController
  {
    private readonly IMonthlySalaryComponentService _monthlySalaryComponentService;
    private readonly ISalaryComponentService _salaryComponentService;
    private readonly IPayslipService _payslipService;
    private readonly IPayrollService _payrollService;
    private readonly IEmployeeService _employeeService;
    private readonly IFieldService _fieldService;
    private readonly IDocumentService _documentService;
    private readonly IFormulaService _formulaService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IPayrollComponentService _payrollComponentService;

    public SalaryComponentController(IMonthlySalaryComponentService monthlySalaryComponentService,
      ISalaryComponentService salaryComponentService, IPayslipService payslipService, IPayrollService payrollService, IEmployeeService employeeService
      , IFieldService fieldService, IPayrollComponentService payrollComponentService,
      IDocumentService documentService, IFormulaService formulaService, IFormulaTypeService formulaTypeService,
      IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService)
    {
      _monthlySalaryComponentService = monthlySalaryComponentService;
      _salaryComponentService = salaryComponentService;
      _payrollService = payrollService;
      _employeeService = employeeService;
      _fieldService = fieldService;
      _documentService = documentService;
      _formulaService = formulaService;
      _formulaTypeService = formulaTypeService;
      _fieldTypeService = fieldTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _payslipService = payslipService;
      _payrollComponentService = payrollComponentService;
    }

    [Route("salarycomponent")]
    [HttpPost]
    public IActionResult Create([FromBody] SalaryComponentVM viewModel)
    {
      try
      {
        _salaryComponentService.Create(ModelMapper.ConvertToModel(viewModel));
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [Route("salarycomponents")]
    [HttpPost]
    public IActionResult Create([FromBody] List<SalaryComponentVM> viewModel)
    {
      try
      {
        ICollection<SalaryComponentVM> result = new List<SalaryComponentVM>();
        var models = ModelMapper.ConvertToCreateViewModel(_salaryComponentService.GetAll().ToList());
        foreach (var vmodel in viewModel)
        {
          if (!models.Contains(ModelMapper.ConvertToViewModel(vmodel)))
          {
            result.Add(vmodel);
          }
        }
        _salaryComponentService.Create(ModelMapper.ConvertToModel(result));
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(409, e);
      }
    }

    [Route("monthlysalarycomponent")]
    [HttpPost]
    public IActionResult Create([FromBody] MonthlySalaryComponentVM viewModel)
    {
      try
      {
        var model = new MonthlySalaryComponent();
        model.Value = viewModel.Value;
        model.FieldId = viewModel.FieldId;
        model.PayslipId = _payslipService.Get(_ => _.EmpId == viewModel.EmpId && _.PayrollId == viewModel.PayrollId).Id;
        _monthlySalaryComponentService.Create(model);
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [Route("monthlysalarycomponents")]
    [HttpPost]
    public IActionResult Create([FromBody] List<MonthlySalaryComponentVM> viewModels)
    {
      try
      {

        var cmodels = new List<MonthlySalaryComponent>();
        var umodels = new List<MonthlySalaryComponent>();
        var fieldList = GetFieldByDoc(true);
        foreach (var viewModel in viewModels)
        {
          var checkfield = fieldList.FirstOrDefault(_ => _.Id == viewModel.FieldId);
          if (checkfield != null)
          {
            var checkpayslip = _payslipService.Get(_ => _.PayrollId == viewModel.PayrollId && _.EmpId == viewModel.EmpId);
            if (checkpayslip != null)
            {
              var model = _monthlySalaryComponentService.Get(_ => _.PayslipId == checkpayslip.Id && _.FieldId == viewModel.FieldId);
              if (model != null)
              {
                model.Value = viewModel.Value;
                umodels.Add(model);
              }
              else
              {
                var nmodel = new MonthlySalaryComponent();
                nmodel.FieldId = viewModel.FieldId;
                nmodel.PayslipId = checkpayslip.Id;
                nmodel.Value = viewModel.Value;
                cmodels.Add(nmodel);
              }

            }
          }
          else return BadRequest("Nội dung tải lên không đúng công thức");
        }
        if (cmodels != null)
        {
          _monthlySalaryComponentService.Create(cmodels);
        }
        if (umodels != null)
        {
          _monthlySalaryComponentService.Update(umodels);
        }
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(409, e);
      }
    }

    [Route("payrollcomponents")]
    [HttpPost]
    public IActionResult Create([FromBody] List<PayrollComponentVM> viewModels)
    {
      try
      {
        var cmodels = new List<PayrollComponent>();
        var umodels = new List<PayrollComponent>();
        var fieldList = GetPayrollFieldByDoc();
        foreach (var viewModel in viewModels)
        {
          var checkfield = fieldList.FirstOrDefault(_ => _.Id == viewModel.FieldId);
          if (checkfield != null)
          {
              var model = _payrollComponentService.Get(_ => _.PayrollId == viewModel.PayrollId && _.FieldId == viewModel.FieldId);
              if (model != null)
              {
                model.Value = viewModel.Value;
                umodels.Add(model);
              }
              else
              {
                var nmodel = new PayrollComponent();
                nmodel.FieldId = viewModel.FieldId;
                nmodel.PayrollId = viewModel.PayrollId;
                nmodel.Value = viewModel.Value;
                cmodels.Add(nmodel);
              }

          }
          else return BadRequest("Nội dung tải lên không đúng công thức");
        }
        if (cmodels != null)
        {
          _payrollComponentService.Create(cmodels);
        }
        if (umodels != null)
        {
          _payrollComponentService.Update(umodels);
        }
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(409, e);
      }
    }

    private ICollection<Field> GetFieldByDoc(bool isMonthly)
    {
      var document = _documentService.Get(_ => _.Status == DocStatus.Active, _ => _.Formula);
      var formula = document.Formula;
      ICollection<Field> resultS = new List<Field>();
      ICollection<Field> resultM = new List<Field>();
      var listField = GetFieldByFormula(formula);
      if (listField.Count != 0)
      {
        foreach (var field in listField)
        {
          if (field.IsMonthlyComponent && field.Name != "Dư nợ" && field.DataType != "payroll")
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

    // get payroll field
    private ICollection<Field> GetPayrollFieldByDoc()
    {
      var document = _documentService.Get(_ => _.Status == DocStatus.Active, _ => _.Formula);
      var formula = document.Formula;
      ICollection<Field> result = new List<Field>();
      var listField = GetFieldByFormula(formula);
      if (listField.Count != 0)
      {
        foreach (var field in listField)
        {
          if (field.DataType == "payroll")
          {
            result.Add(field);
          }
        }
      }
      else return null;
      return result;
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
            // use hashset to not add duplicate element
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
          case 4:
            break;
        }
      });
      return list;
    }
  }

  public class PayrollComponentVM
  {
    public int PayrollId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; }
  }
}
