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
  public class EmployeeController : _BaseController
  {
    private readonly IPayslipService _payslipService;
    private readonly IPayrollService _payrollService;
    private readonly IEmployeeService _employeeService;
    private readonly ISalaryComponentService _salaryComponentService;
    private readonly IFieldService _fieldService;
    private readonly IPositionService _positionService;
    private readonly IPositionDetailService _positionDetailService;
    private readonly IAccountService _accountService;
    public EmployeeController(IEmployeeService employeeService, ISalaryComponentService salaryComponentService, IPayslipService payslipService,
      IFieldService fieldService, IPositionService positionService, IPositionDetailService positionDetailService, IPayrollService payrollService, IAccountService accountService)
    {
      _employeeService = employeeService;
      _salaryComponentService = salaryComponentService;
      _fieldService = fieldService;
      _positionService = positionService;
      _positionDetailService = positionDetailService;
      _payrollService = payrollService;
      _payslipService = payslipService;
      _accountService = accountService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] EmployeeCreateVM viewModel)
    {
      try
      {
        var entity = ModelMapper.ConvertToModel(viewModel);
        var positionDetail = ModelMapper.ConvertToModel(viewModel.PositionDetailCreateVM);
        _employeeService.Create(entity, positionDetail);
        SalaryComponent positionSalary = new SalaryComponent();
        positionSalary.EmpId = entity.Code;
        positionSalary.FieldId = _fieldService.Get(_ => _.Name == "Chức vụ").Id;
        positionSalary.ApplyDate = positionDetail.ApplyDate;
        positionSalary.Value = _positionDetailService.Get(_ => _.Id == positionDetail.Id, _ => _.Position).Position.Name;
        _salaryComponentService.Create(positionSalary);
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }

    [HttpPut]
    public IActionResult Update([FromBody] EmployeeCreateVM viewModel)
    {
      try
      {
        var entity = ModelMapper.ConvertToModel(viewModel);
        var positionDetail = ModelMapper.ConvertToModel(viewModel.PositionDetailCreateVM);
        _employeeService.Update(entity);

        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }

    public class empVM
    {
      public empVM()
      {
      }

      public empVM(Account account, Employee emp)
      {
        this.account = account;
        this.emp = emp;
      }

      public Account account { get; set; }
      public Employee emp { get; set; }
    }

    [HttpGet]
    [Route("employees")]
    public IActionResult GetAll()
    {
      try
      {
        var result = _employeeService.GetAll(_ => _.PositionDetails, _ => _.Department).ToList();
        return Ok(result);
      }
      catch (Exception e)
      {
        return StatusCode(500);
      }
    }

    [HttpGet]
    [Route("manage-employees")]
    public IActionResult GetAllManageEmps()
    {
      try
      {
        List<empVM> result = new List<empVM>();
        List<Account> accounts = _accountService.GetAll().ToList();
        foreach (var acc in accounts)
        {
          Employee emp = _employeeService.GetAll().Where(_ => _.Code == acc.Code).FirstOrDefault();
          result.Add(new empVM(acc, emp));
        }
        //var result = _employeeService.GetAll(_ => _.PositionDetails, _ => _.Department).ToList();
        return Ok(result);
      }
      catch (Exception e)
      {
        return StatusCode(500);
      }

    }

    [HttpGet]
    [Route("payslip-employee")]
    //[Authorize]
    public IActionResult GetAllFor(string employeeId, int month, int year)
    {
      try
      {
        var payrollId = _payrollService.Get(_ => _.Month == month && _.Year == year).Id;
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



  }
}
