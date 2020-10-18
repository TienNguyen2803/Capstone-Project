using BusinessLogic.Define;
using CapstoneUI.Firebase;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class PayrollController : _BaseController
  {
    private readonly IPayrollService _payrollService;
    private readonly IEmployeeService _employeeService;
    private readonly IMonthlySalaryComponentService _monthlyService;
    private readonly ISalaryComponentService _salaryService;
    private readonly IPayrollComponentService _payrollComponentService;
    private readonly IFieldService _fieldService;
    private readonly IDocumentService _documentService;
    private readonly IFormulaService _formulaService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IPayslipService _payslipService;

    public PayrollController(IPayrollService payrollService, IEmployeeService employeeService
      , IMonthlySalaryComponentService monthlySalaryComponentService, IFieldService fieldService,
      IDocumentService documentService, IFormulaService formulaService, IFormulaTypeService formulaTypeService,
      IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService, IPayslipService payslipService,
      ISalaryComponentService salaryService, IPayrollComponentService payrollComponentService
      )
    {
      _payrollService = payrollService;
      _employeeService = employeeService;
      _monthlyService = monthlySalaryComponentService;
      _fieldService = fieldService;
      _documentService = documentService;
      _formulaService = formulaService;
      _formulaTypeService = formulaTypeService;
      _fieldTypeService = fieldTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _payslipService = payslipService;
      _salaryService = salaryService;
      _payrollComponentService = payrollComponentService;
    }
    public PayrollController(IPayrollService payrollService, IDocumentService documentService)
    {
      _payrollService = payrollService;
      _documentService = documentService;
    }

    [HttpPost]
    [Route("payroll")]
    public IActionResult Create(int m, int y)
    {
      try
      {
        var currentDoc = _documentService.Get(x => x.Status == DocStatus.Priority, _ => _.Formula.FormulaDetails);
        if (currentDoc == null)
        {
          currentDoc = _documentService.Get(x => x.Status == DocStatus.Active, _ => _.Formula.FormulaDetails);
        }

        if (currentDoc != null)
        {
          var month = m;
          var year = y;
          int day;
          DateTimeOffset fromdate;
          DateTimeOffset todate;
          DateTimeOffset paydate;
          string from;
          string to;
          string pay;

          string[] formats = new string[] { "MM/dd/yyyy", "MM/d/yyyy", "M/d/yyyy", "M/dd/yyyy" };

          if (month == 1)
          {
            from = 12 + "/" + currentDoc.CloseDay + "/" + (year - 1);
            to = month + "/" + (currentDoc.CloseDay - 1) + "/" + year;
          }
          else
          {
            from = (month - 1) + "/" + currentDoc.CloseDay + "/" + year;
            to = month + "/" + (currentDoc.CloseDay - 1) + "/" + year;
            if (currentDoc.CloseDay - 1 < 27)
            {
              switch (month)
              {
                case 2:
                  to = month + "/" + 28 + "/" + year;
                  break;
                case 3:
                  from = month + "/" + 1 + "/" + year;
                  break;
                default:
                  break;
              }
            }
          }

          pay = (month + 1) + "/" + (currentDoc.Deadline + 1) + "/" + year;

          if (currentDoc.Deadline >= currentDoc.CloseDay)
          {
            if (month == 2 && currentDoc.Deadline == 28)
            {
              pay = (month + 1) + "/" + 1 + year;
            }

            if ((month == 4 || month == 6 || month == 9 || month == 11))
            {
              pay = month + "/" + (currentDoc.Deadline + 1) + "/" + year;
            }
          }

          if (DateTimeOffset.TryParseExact(from, formats, System.Globalization.CultureInfo.InvariantCulture,
                  System.Globalization.DateTimeStyles.None, out fromdate))
          {
          }
          if (DateTimeOffset.TryParseExact(to, formats, System.Globalization.CultureInfo.InvariantCulture,
                 System.Globalization.DateTimeStyles.None, out todate))
          {
          }
          if (DateTimeOffset.TryParseExact(pay, formats, System.Globalization.CultureInfo.InvariantCulture,
                 System.Globalization.DateTimeStyles.None, out paydate))
          {
          }

          var payroll = new PayrollCreateVM
          {
            Month = m,
            Year = y,
            FromDate = fromdate,
            ToDate = todate,
            PayDate = paydate
          };

          _payrollService.CreatePayroll(ModelMapper.ConvertToModel(payroll), currentDoc);
          return Ok();
        }


        return BadRequest("Không có quyết định hiện hành");
      }

      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    private delegate Task<IActionResult> CreatePayroll();

    [HttpGet]
    [Route("payrolls")]
    public IActionResult GetAll()
    {
      try
      {
        (new CreatePayroll(new ScheduleController(_documentService, _payslipService, _payrollService, _employeeService).CheckData))();

        var result = _payrollService.GetAll(_ => _.Document).OrderByDescending(_ => _.Year).ThenByDescending(_ => _.Month)
          .ThenBy(_ => _.Status).ToList();

        return Ok(ModelMapper.ConvertToViewModel(result));
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id)
    {
      try
      {
        var payroll = _payrollService.Get(_ => _.Id == id, _ => _.Payslips, _ => _.Document);
        var formula = _formulaService.Get(_ => _.Id == payroll.Document.FormulaId);
        var output = new PayrollGetVM
        {
          Id = payroll.Id,
          Month = payroll.Month,
          Year = payroll.Year,
          Payslips = payroll.Payslips.Select(x => new PayslipGetVM
          {
            Id = x.Id,
            Amount = x.Amount,
            EmpId = x.EmpId,
          }).ToList()
        };

        foreach (var payslip in output.Payslips)
        {
          var employee = _employeeService.Get(_ => _.Code == payslip.EmpId, _ => _.SalaryComponents);
          var listField = GetFieldByFormula(formula);
          ICollection<SalaryComponent> listSalary = new List<SalaryComponent>();
          foreach (var field in listField)
          {
            var salary = employee.SalaryComponents.Where(_ => _.FieldId == field.Id && _.ApplyDate <= payroll.ToDate)
              .OrderByDescending(_ => _.ApplyDate).FirstOrDefault();
            if (salary != null)
            {
              listSalary.Add(salary);
            }
          }
          payslip.Employee = new EmployeeGetVM
          {
            Code = employee.Code,
            Name = employee.Fullname,
            SalaryComponents = listSalary.Select(_ => new SalaryComponentGetVM
            {
              FieldId = _.FieldId,
              Value = _.Value
            }).ToList()
          };

          foreach (var salaryComponent in payslip.Employee.SalaryComponents)
          {
            salaryComponent.FieldName = _fieldService.Get(_ => _.Id == salaryComponent.FieldId).Name;
          }

          payslip.MonthlySalaryComponents = _monthlyService.GetAll(_ => _.Field).Where(_ => _.PayslipId == payslip.Id).ToHashSet()
            .Select(_ => new SalaryComponentGetVM
            {
              FieldName = _.Field.Name,
              Value = _.Value
            }).ToList();
        }
        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpPut]
    [Route("publish-payroll")]
    public async Task <IActionResult> UpdateStatus(int id)
    {
      var check = true;
      try
      {

        #region
        List<Employee> employees = new List<Employee>();
        #endregion

        var payroll = _payrollService.Get(_ => _.Id == id, _ => _.Payslips, _ => _.Document);
        var publishedPayroll = _payrollService.Get(_ =>
        _.DocId == payroll.DocId
        && (_.Status == PayrollStatus.Published
        || _.Status == PayrollStatus.Completed)
        && _.Month == payroll.Month
        && _.Year == payroll.Year);

        if (publishedPayroll == null)
        {
          foreach (var payslip in payroll.Payslips)
          {
            var psl = _payslipService.Get(_ => _.Id == payslip.Id, _ => _.Employee);

            #region khoinkt edit
            var emp = _employeeService.Get(_ => _.Code == psl.EmpId);
            #endregion

            var formula = _formulaService.Get(_ => _.Document.Id == payroll.DocId, _ => _.FormulaDetails);
            try
            {
              psl.Amount = _payslipService.GetFormula(formula, psl.Id, psl.EmpId).Value;
              _payslipService.Update(psl);
            }
            catch (Exception)
            {
              check = false;
              employees.Add(emp);
              //log ở đây nè Khôi
            }
          }
          if (check == false)
          {
            return BadRequest(employees);
          }
          #region Send Notify
          var emps = _employeeService.GetAll(_ => _.Account.Role).Where(_ => _.Account.Role.Name != "admin").ToList();
          foreach (var emp in emps)
          {
            await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
            {
              Role = emp.Account.Role.Name,
              IsReading = false,
              Status = NotiStatus.EMP_NEWPAYSLIP,
              Month = payroll.Month,
              Year = payroll.Year,
              CreatedDate = DateTimeOffset.Now
            });
          }
          #endregion

          payroll.Status = PayrollStatus.Published;
          _payrollService.Update(payroll);


          return Ok();
        }
        return BadRequest("Đã có bảng lương đăng tải");
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    private PayrollGetVM getPayrollByID(int id)
    {
      var payroll = _payrollService.Get(_ => _.Id == id, _ => _.Payslips, _ => _.Document);
      var formula = _formulaService.Get(_ => _.Id == payroll.Document.FormulaId);
      var output = new PayrollGetVM
      {
        Id = payroll.Id,
        Month = payroll.Month,
        Year = payroll.Year,
        Payslips = payroll.Payslips.Select(x => new PayslipGetVM
        {
          Id = x.Id,
          Amount = x.Amount,
          EmpId = x.EmpId,
        }).ToList()
      };

      foreach (var payslip in output.Payslips)
      {
        var employee = _employeeService.Get(_ => _.Code == payslip.EmpId, _ => _.SalaryComponents);
        var listField = GetFieldByFormula(formula);
        ICollection<SalaryComponent> listSalary = new List<SalaryComponent>();
        foreach (var field in listField)
        {
          var salary = employee.SalaryComponents.Where(_ => _.FieldId == field.Id && _.ApplyDate <= payroll.ToDate)
            .OrderByDescending(_ => _.ApplyDate).FirstOrDefault();
          if (salary != null)
          {
            listSalary.Add(salary);
          }
        }
        payslip.Employee = new EmployeeGetVM
        {
          Code = employee.Code,
          Name = employee.Fullname,
          SalaryComponents = listSalary.Select(_ => new SalaryComponentGetVM
          {
            FieldId = _.FieldId,
            Value = _.Value
          }).ToList()
        };

        foreach (var salaryComponent in payslip.Employee.SalaryComponents)
        {
          salaryComponent.FieldName = _fieldService.Get(_ => _.Id == salaryComponent.FieldId).Name;
        }

        payslip.MonthlySalaryComponents = _monthlyService.GetAll(_ => _.Field).Where(_ => _.PayslipId == payslip.Id).ToHashSet()
          .Select(_ => new SalaryComponentGetVM
          {
            FieldName = _.Field.Name,
            Value = _.Value
          }).ToList();
      }
      return output;
    }

    [HttpGet]
    [Route("EmpSalaryComponents")]
    public IActionResult GetEmpSalaryComponent()
    {
      try
      {
        var doc = _documentService.Get(_ => _.Status == DocStatus.Active, _ => _.Formula);
        var output = new SalaryComponentGAVM
        {
          docId = doc.Id
        };

        if (doc != null)
        {
          output.salaryFields = GetFieldByDoc(false).Select(f => new FieldElementVM
          {
            Id = f.Id,
            Name = f.Name,
            Type = 1
          }).ToList();

          var emplist = _employeeService.GetAll(_ => _.SalaryComponents, _ => _.Department, _ => _.PositionDetails).ToList();

          if (emplist.Count != 0)
          {
            output.emps = emplist.Select(e => new EmployeeGAVM
            {
              Code = e.Code,
              Fullname = e.Fullname,
              Department = e.Department.DepName + "/" + e.Department.DepOffice,
              Position = e.SalaryComponents.FirstOrDefault(_ => _.FieldId == 1).Value
            }).ToList();

            foreach (var emp in output.emps)
            {
              var employee = _employeeService.Get(_ => _.Code == emp.Code, _ => _.SalaryComponents);
              ICollection<SalaryComponent> listSalary = new List<SalaryComponent>();
              var now = DateTimeOffset.Now;
              foreach (var field in output.salaryFields)
              {
                var salary = employee.SalaryComponents.Where(_ => _.FieldId == field.Id && _.ApplyDate <= now)
                  .OrderByDescending(_ => _.ApplyDate).FirstOrDefault();
                if (salary != null)
                {
                  listSalary.Add(salary);
                }
              }
              emp.SalaryComponents = listSalary.Select(s => new SalaryComponentGetVM
              {
                FieldId = s.FieldId,
                Value = s.Value
              }).ToList();

              foreach (var salaryComponent in emp.SalaryComponents)
              {
                salaryComponent.FieldName = _fieldService.Get(_ => _.Id == salaryComponent.FieldId).Name;
              }
            }
          }
          else return BadRequest("Không có thông tin nhân viên nào trong hệ thống");
        }
        else return BadRequest("Không có quyết định nào trong hệ thống");
        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    public class SalaryProgressVM
    {
      public SalaryProgressVM(double progress, List<Employee> employees)
      {
        Progress = progress;
        Employees = employees;
      }

      public double Progress { get; set; }
      public List<Employee> Employees { get; set; }
    }

    private bool salaryComponentsContainField(List<SalaryComponent> salaryComponents, Field field)
    {
      bool result = false;
      foreach (var salaryComponent in salaryComponents)
      {
        if (salaryComponent.FieldId == field.Id)
        {
          result = true;
          break;
        }
      }
      return result;
    }

    [HttpGet]
    [Route("CheckSalaryProgress")]
    public IActionResult CheckSalaryProgress()
    {
      try
      {
        List<Employee> employeesWorking = _employeeService.GetAll().Where(_ => _.IsWorking == true).ToList();
        List<SalaryComponent> salaryComponents = _salaryService.GetAll().ToList();

        HashSet<Employee> employeesMissingSalary = new HashSet<Employee>();
        double salaryProgress = 0;

        // get salary components
        List<Field> salaryFields = GetFieldByDoc(false).ToList();


        // check salary progress
        foreach (var employee in employeesWorking)
        {
          // tổng các salary component của nhân viên đó
          List<SalaryComponent> tempSalaryComponents = salaryComponents.Where(_ => _.EmpId == employee.Code).ToList();

          // xét từng salary field trong công thức để xem nhân viên đó đã có đủ các salary component chưa
          foreach (var field in salaryFields)
          {
            if (salaryComponentsContainField(tempSalaryComponents, field) == false)
            {
              employeesMissingSalary.Add(employee);
              break;
            }
          }
        }
        if (employeesWorking.Count > 0)
        {
          salaryProgress = (double)(employeesWorking.Count - employeesMissingSalary.Count) / (double)employeesWorking.Count;
        }
        else
        {
          salaryProgress = -1;
        }

        return Ok(new SalaryProgressVM(Math.Round(salaryProgress, 2), employeesMissingSalary.ToList()));
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    public class MonthlySalaryProgressVM
    {
      public MonthlySalaryProgressVM(double monthlyProgress, List<Employee> employees, double payrollProgress, List<Field> fieldPayrolls)
      {
        MonthlyProgress = monthlyProgress;
        Employees = employees;
        PayrollProgress = payrollProgress;
        FieldPayrolls = fieldPayrolls;
      }

      public double MonthlyProgress { get; set; }

      public List<Employee> Employees { get; set; }

      public double PayrollProgress { get; set; }

      public List<Field> FieldPayrolls { get; set; }

    }

    private bool monthlyComponentsContainField(List<MonthlySalaryComponent> monthlyComponents, Field field)
    {
      bool result = false;
      foreach (var monthlyComponent in monthlyComponents)
      {
        if (monthlyComponent.FieldId == field.Id)
        {
          result = true;
          break;
        }
      }
      return result;
    }

    private bool payrollComponentsContainField(List<PayrollComponent> payrollComponents, Field field)
    {
      bool result = false;
      foreach (var payrollComponent in payrollComponents)
      {
        if (payrollComponent.FieldId == field.Id)
        {
          result = true;
          break;
        }
      }
      return result;
    }

    [HttpGet]
    [Route("CheckMonthlyProgress")]
    public IActionResult CheckMonthlyProgress(int payrollId)
    {
      try
      {
        Payroll payroll = _payrollService.GetAll().Where(_ => _.Id == payrollId).FirstOrDefault();
        List<Payslip> payslips = _payslipService.GetAll().Where(_ => _.PayrollId == payroll.Id).ToList();
        //List<Employee> employeesWorking = _employeeService.GetAll().Where(_ => _.IsWorking == true).ToList();

        List<MonthlySalaryComponent> monthlySalaryComponents = _monthlyService.GetAll().ToList();

        HashSet<Employee> employeesMissingMonthly = new HashSet<Employee>();
        double monthlyProgress = 0;

        List<Field> payrollFieldMissing = new List<Field>();
        double payrollProgress = 0;

        // get monthly components
        List<Field> salaryFields = GetFieldByDoc(true).ToList();


        // check salary progress
        foreach (var payslip in payslips)
        {
          // tổng các monthly component của nhân viên đó
          List<MonthlySalaryComponent> tempMonthlyComponents = monthlySalaryComponents.Where(_ => _.PayslipId == payslip.Id).ToList();

          // xét từng salary field trong công thức để xem nhân viên đó đã có đủ các salary component chưa
          foreach (var field in salaryFields)
          {
            if (field.DataType != "payroll" && monthlyComponentsContainField(tempMonthlyComponents, field) == false)
            {
              Employee employee = _employeeService.Get(_ => _.Code == payslip.EmpId);
              employeesMissingMonthly.Add(employee);
              break;
            }
          }
        }
        monthlyProgress = (double)(payslips.Count - employeesMissingMonthly.Count) / (double)payslips.Count;

        //check payroll component progress
        List<Field> fields = GetFieldByDoc(true).ToList();
        double payrollComponentsCount = 0;
        List<PayrollComponent> payrollComponents = _payrollComponentService.GetAll().Where(_ => _.PayrollId == payrollId).ToList();
        foreach (var field in fields)
        {
          if (field.DataType == "payroll")
          {
            if (payrollComponentsContainField(payrollComponents, field) == false)
            {
              payrollFieldMissing.Add(field);
            }
            payrollComponentsCount++;
          }
        }

        if (payrollComponentsCount > 0)
        {
          payrollProgress = (double)(payrollComponentsCount - payrollFieldMissing.Count) / (double)payrollComponentsCount;
        }
        else
        {
          payrollProgress = -1;
        }

        return Ok(new MonthlySalaryProgressVM(Math.Round(monthlyProgress, 2), employeesMissingMonthly.ToList(), payrollProgress, payrollFieldMissing));
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpGet]
    [Route("DownloadPayroll")]
    public IActionResult DownloadPayroll(int payrollId)
    {
      try
      {
        var pr = getPayrollByID(payrollId);
        var stream = new MemoryStream();

        using (var package = new ExcelPackage(stream))
        {
          var workSheet = package.Workbook.Worksheets.Add("Sheet1");
          int i = 1;
          foreach (var payslip in pr.Payslips)
          {
            int j = 2;
            workSheet.Cells[1, 1].Value = "Tên nhân viên";
            workSheet.Cells[1, 1].Style.Font.Bold = true;
            workSheet.Cells[1, 1].Style.Font.Size = 14;

            workSheet.Cells[1, 2].Value = "Mã nhân viên";
            workSheet.Cells[1, 2].Style.Font.Bold = true;
            workSheet.Cells[1, 2].Style.Font.Size = 14;

            workSheet.Cells[i + 1, 1].Value = payslip.Employee.Code;
            workSheet.Cells[i + 1, 2].Value = payslip.Employee.Name;
            // get salary component
            foreach (var Scomp in payslip.Employee.SalaryComponents)
            {
              // id
              workSheet.Cells[1, j + 1].Value = Scomp.FieldName;
              workSheet.Cells[1, j + 1].Style.Font.Bold = true;
              workSheet.Cells[1, j + 1].Style.Font.Size = 14;

              // value
              workSheet.Cells[i + 1, j + 1].Value = UtilityService.CommaForBigNum(Scomp.Value);
              workSheet.Cells[i + 1, j + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
              j++;
            }
            // get monthly salary component
            foreach (var Mcomp in payslip.MonthlySalaryComponents)
            {
              // id
              workSheet.Cells[1, j + 1].Value = Mcomp.FieldName;
              workSheet.Cells[1, j + 1].Style.Font.Bold = true;
              workSheet.Cells[1, j + 1].Style.Font.Size = 14;

              workSheet.Cells[i + 1, j + 1].Value = UtilityService.CommaForBigNum(Mcomp.Value);
              workSheet.Cells[i + 1, j + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
              j++;
            }
            // id = thuc lanh
            workSheet.Cells[1, j + 1].Value = "Thực lãnh";
            workSheet.Cells[1, j + 1].Style.Font.Bold = true;
            workSheet.Cells[1, j + 1].Style.Font.Size = 14;
            // cast ve int de ko bi phan so phia sau
            workSheet.Cells[i + 1, j + 1].Value = UtilityService.CommaForBigNum(((int)payslip.Amount + ""));
            workSheet.Cells[i + 1, j + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            i++;
          }
          workSheet.Cells.AutoFitColumns();

          package.Save();
        }
        stream.Position = 0;
        // file name definition
        string tempMonthYear = pr.Month + "-" + pr.Year;
        string excelName = $"Bảng Lương " + tempMonthYear + $".xlsx";

        //return File(stream, "application/octet-stream", excelName);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
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
      var document = _documentService.Get(_ => _.Status == DocStatus.Active, _ => _.Formula);
      var formula = document.Formula;
      ICollection<Field> resultS = new List<Field>();
      ICollection<Field> resultM = new List<Field>();
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

    [HttpGet]
    [Route("GetExcelTemplate")]
    public IActionResult ExportExcelTemplate(int status)
    {
      var stream = new MemoryStream();
      string excelName = "";
      // get employee
      List<Employee> employees = new List<Employee>();

      employees = _employeeService.GetAll().Where(_ => _.IsWorking == true).ToList();

      switch (status)
      {
        case 1:
          // salary component
          excelName = "Mẫu thêm thông tin lương nhân viên.xlsx";
          // get salary fields
          List<Field> salaryFields = (List<Field>)GetFieldByDoc(false);
          using (var package = new ExcelPackage(stream))
          {
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            // init
            workSheet.Cells[1, 1].Value = "ĐẦU LƯƠNG CỐ ĐỊNH";
            workSheet.Cells[3, 1].Value = "MÃ";
            workSheet.Cells[3, 2].Value = "HỌ VÀ TÊN";

            // load employee
            int i = 3;
            foreach (var emp in employees)
            {
              i++;
              workSheet.Cells[i, 1].Value = emp.Code;
              workSheet.Cells[i, 2].Value = emp.Fullname;
            }

            // load field
            int j = 2;
            foreach (var field in salaryFields)
            {
              j++;
              workSheet.Cells[2, j].Value = field.Id;
              workSheet.Cells[3, j].Value = field.Name;
            }
            // hide row contain ID
            workSheet.Cells.AutoFitColumns();
            workSheet.Row(2).Hidden = true;
            package.Save();
          }
          break;
        case 2:
          // monthly component
          excelName = "Mẫu thêm thông tin lương hàng tháng.xlsx";
          // get salary fields
          List<Field> monthlyFields = (List<Field>)GetFieldByDoc(true);
          using (var package = new ExcelPackage(stream))
          {
            int startRow = 2;
            int startCol = 3;
            int hideRow = startRow;
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 2].Value = "THÔNG TIN LƯƠNG HÀNG THÁNG";
            // init payroll
            foreach (var field in monthlyFields)
            {
              if (field.DataType == "payroll")
              {
                startRow++;
                workSheet.Cells[startRow, 1].Value = field.Id;
                workSheet.Cells[startRow, 2].Value = field.Name;
              }
            }

            startRow += 3;
            // init monthly
            workSheet.Cells[startRow - 1, 2].Value = "monthly";
            workSheet.Cells[startRow, 2].Value = "MÃ";
            workSheet.Cells[startRow, 3].Value = "HỌ VÀ TÊN";

            // load field
            hideRow = startRow - 1;
            foreach (var field in monthlyFields)
            {
              if (field.DataType != "payroll")
              {
                startCol++;
                workSheet.Cells[startRow - 1, startCol].Value = field.Id;
                workSheet.Cells[startRow, startCol].Value = field.Name;
              }
            }

            // load employee
            foreach (var emp in employees)
            {
              startRow++;
              workSheet.Cells[startRow, 2].Value = emp.Code;
              workSheet.Cells[startRow, 3].Value = emp.Fullname;
            }

            // hide row contain ID
            workSheet.Cells.AutoFitColumns();
            workSheet.Row(hideRow).Hidden = true;
            workSheet.Column(1).Hidden = true;
            package.Save();
          }
          break;
      }
      stream.Position = 0;
      //return File(stream, "application/octet-stream", excelName);
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }

    [HttpPost]
    [Route("GetExcelTemplate2")]
    public IActionResult ExportExcelTemplate(int status, [FromBody] List<Employee> emps)
    {
      var stream = new MemoryStream();
      string excelName = "";
      // get employee
      List<Employee> employees = new List<Employee>();

      if (emps.Count > 0)
      {
        employees = emps;
      }
      else
      {
        employees = _employeeService.GetAll().Where(_ => _.IsWorking == true).ToList();
      }


      switch (status)
      {
        case 1:
          // salary component
          excelName = "Mẫu thêm thông tin lương nhân viên.xlsx";
          // get salary fields
          List<Field> salaryFields = (List<Field>)GetFieldByDoc(false);
          using (var package = new ExcelPackage(stream))
          {
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            // init
            workSheet.Cells[1, 1].Value = "ĐẦU LƯƠNG CỐ ĐỊNH";
            workSheet.Cells[3, 1].Value = "MÃ";
            workSheet.Cells[3, 2].Value = "HỌ VÀ TÊN";

            // load employee
            int i = 3;
            foreach (var emp in employees)
            {
              i++;
              workSheet.Cells[i, 1].Value = emp.Code;
              workSheet.Cells[i, 2].Value = emp.Fullname;
            }

            // load field
            int j = 2;
            foreach (var field in salaryFields)
            {
              j++;
              workSheet.Cells[2, j].Value = field.Id;
              workSheet.Cells[3, j].Value = field.Name;
            }
            // hide row contain ID
            workSheet.Cells.AutoFitColumns();
            workSheet.Row(2).Hidden = true;
            package.Save();
          }
          break;
        case 2:
          // monthly component
          excelName = "Mẫu thêm thông tin lương hàng tháng.xlsx";
          // get salary fields
          List<Field> monthlyFields = (List<Field>)GetFieldByDoc(true);
          using (var package = new ExcelPackage(stream))
          {
            int startRow = 2;
            int startCol = 3;
            int hideRow = startRow;
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 2].Value = "THÔNG TIN LƯƠNG HÀNG THÁNG";
            // init payroll
            foreach (var field in monthlyFields)
            {
              if (field.DataType == "payroll")
              {
                startRow++;
                workSheet.Cells[startRow, 1].Value = field.Id;
                workSheet.Cells[startRow, 2].Value = field.Name;
              }
            }

            startRow += 3;
            // init monthly
            workSheet.Cells[startRow - 1, 2].Value = "monthly";
            workSheet.Cells[startRow, 2].Value = "MÃ";
            workSheet.Cells[startRow, 3].Value = "HỌ VÀ TÊN";

            // load field
            hideRow = startRow - 1;
            foreach (var field in monthlyFields)
            {
              if (field.DataType != "payroll")
              {
                startCol++;
                workSheet.Cells[startRow - 1, startCol].Value = field.Id;
                workSheet.Cells[startRow, startCol].Value = field.Name;
              }
            }

            // load employee
            foreach (var emp in employees)
            {
              startRow++;
              workSheet.Cells[startRow, 2].Value = emp.Code;
              workSheet.Cells[startRow, 3].Value = emp.Fullname;
            }

            // hide row contain ID
            workSheet.Cells.AutoFitColumns();
            workSheet.Row(hideRow).Hidden = true;
            workSheet.Column(1).Hidden = true;
            package.Save();
          }
          break;
      }
      stream.Position = 0;
      //return File(stream, "application/octet-stream", excelName);
      return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
    }

    [HttpDelete]
    [Route("payroll")]
    public IActionResult DeletePayrollByID(int payrollId)
    {
      try
      {
        var payroll = _payrollService.Get(_ => _.Id == payrollId, _ => _.Payslips, _ => _.Document);

        if (payroll.Status != PayrollStatus.Published)
        {
          var payplips = _payslipService.GetAll(_ => _.MonthlySalaryComponents).Where(_ => _.PayrollId == payroll.Id).ToList();
          payplips.ForEach(_ =>
          {
            if (_.MonthlySalaryComponents.Count != 0)
            {
              _monthlyService.Delete(_.MonthlySalaryComponents);
            }
          });

          _payslipService.Delete(payplips);

          _payrollService.Delete(payroll);
          return Ok();
        }
        return StatusCode(409, "Bạn không thể xóa bảng lương đã công khai");
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpPut]
    [Route("payroll")]
    public async Task<IActionResult> ChangeDocument(int m, int y, int docId, bool isChanged)
    {
      try
      {
        var currentDoc = _documentService.Get(_ => _.Status == DocStatus.Active);
        var newDoc = _documentService.Get(_ => _.Id == docId);

        //active doc
        #region active doc
        ///api/Schedule/check-payroll-sch
        var client = new HttpClient();
        var uri = new Uri("http://localhost:3911/api/Schedule/active-doc-sch");
        //var uri = new Uri("http://http://spcs.azurewebsites.net/Schedule/active-doc-sch");
        client.BaseAddress = uri;
        await client.GetAsync(uri);
        #endregion
        if (!isChanged)
        {
          currentDoc.Status = DocStatus.Priority;
          _documentService.Update(currentDoc);
        }
        else
        {
          var payroll = _payrollService.GetAll(_ => _.Document, _ => _.PayrollComponents, _ => _.Payslips).FirstOrDefault(_ => _.Month == m && _.Year == y);
          if (payroll != null)
          {
            if (payroll.PayrollComponents.Count != 0)
            {
              _payrollComponentService.Delete(payroll.PayrollComponents);
            }
            var payplips = _payslipService.GetAll(_ => _.MonthlySalaryComponents).Where(_ => _.PayrollId == payroll.Id).ToList();
            payplips.ForEach(p =>
            {
              if (p.MonthlySalaryComponents.Count != 0)
              {
                _monthlyService.Delete(p.MonthlySalaryComponents);
              }
            });
            _payslipService.Delete(payplips);
            _payrollService.Delete(payroll);
          }
        }
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }
  }
}
