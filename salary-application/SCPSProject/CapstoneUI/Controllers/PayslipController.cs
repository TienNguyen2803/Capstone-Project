using BusinessLogic.Define;
using CapstoneUI.Utils;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PayslipController : _BaseController
  {
    private readonly IPayslipService _payslipService;
    private readonly IPayrollService _payrollService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IConstantTypeService _constantTypeService;
    private readonly IFieldService _fieldService;
    private readonly IReferenceTableDetailService _referenceTableDetailService;
    private readonly IEmployeeService _employeeService;

    public PayslipController(IPayslipService payslipService, IPayrollService payrollService, IConstantTypeService constantTypeService,
      IFormulaTypeService formulaTypeService, IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService,
      IFieldService fieldService,
      IReferenceTableDetailService referenceTableDetailService, IEmployeeService employeeService)
    {
      _payslipService = payslipService;
      _payrollService = payrollService;
      _constantTypeService = constantTypeService;
      _formulaTypeService = formulaTypeService;
      _fieldTypeService = fieldTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _fieldService = fieldService;
      _referenceTableDetailService = referenceTableDetailService;
      _employeeService = employeeService;
    }

    [HttpGet]
    [Route("payslip")]
    //[Authorize]
    public IActionResult GetAllFor(string employeeId, int payrollId)
    {
      try
      {
        //var result = _referenceTableService.Get(x => true, x => x.ReferenceTableDetails);
        // 1 field 2 ref 3 formula 4 constant
        var result = _payslipService.Get(_ => _.EmpId == employeeId && _.PayrollId == payrollId
        , _ => _.Employee, _ => _.Payroll.Document.Formula.FormulaDetails, _ => _.Employee.Department, _ => _.Employee.PositionDetails
          );

        var mainFormula = _payslipService.GetFormula(result.Payroll.Document.Formula, result.Id, employeeId);

        //--update payslip amount
        var payslip = result;
        payslip.Amount = mainFormula.Value;
        _payslipService.Update(payslip);

        var output = new
        {
          employee = new
          {
            fullName = result.Employee.Fullname,
            email = result.Employee.Email,
            address = result.Employee.Address,
            department = result.Employee.Department.DepName
          },
          payroll = new
          {
            month = result.Payroll.Month,
            year = result.Payroll.Year,
            formula = mainFormula
          }
        };

        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpGet]
    [Route("payslips")]
    public IActionResult GetPayslips(int month, int year)
    {
      try
      {

        var payrollId = _payrollService.Get(_ => _.Month == month && _.Year == year).Id;
        var payroll = _payrollService.Get(_ => _.Month == month && _.Year == year, _ => _.Payslips);

        var outputs = new List<Object>();
        foreach (var payslip in payroll.Payslips)
        {
          var result = _payslipService.Get(_ => _.Id == payslip.Id, _ => _.Employee, _ => _.Payroll.Document.Formula.FormulaDetails,
            _ => _.Employee.Department, _ => _.Employee.PositionDetails);
         var output = new
          {
           employee = new
           {
             fullName = result.Employee.Fullname,
             email = result.Employee.Email,
             address = result.Employee.Address,
             department = result.Employee.Department.DepName
           },
           payroll = new
           {
             month = result.Payroll.Month,
             year = result.Payroll.Year,
             formula = _payslipService.GetFormula(result.Payroll.Document.Formula, result.Id, result.EmpId),
           }
         };
          outputs.Add(output);
        }

        //foreach (var empCode in viewModel.Employees)
        //{
        //  var result = _payslipService.Get(_ => _.EmpId == empCode && _.PayrollId == payrollId
        //  , _ => _.Employee, _ => _.Payroll.Document.Formula.FormulaDetails, _ => _.Employee.Department, _ => _.Employee.PositionDetails
        //    );
        //  var payslip = new
        //  {
        //    employee = new
        //    {
        //      fullName = result.Employee.Fullname,
        //      email = result.Employee.Email,
        //      address = result.Employee.Address,
        //      department = result.Employee.Department.DepName
        //    },
        //    payroll = new
        //    {
        //      month = result.Payroll.Month,
        //      year = result.Payroll.Year,
        //      formula = _payslipService.GetFormula(result.Payroll.Document.Formula, result.Id, empCode),
        //    }
        //  };
        //  output.Add(payslip);
        //}
        return Ok(outputs);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);

      }
    }
  }
}
