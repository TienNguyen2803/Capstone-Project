using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessLogic.Define;
using CapstoneUI.Firebase;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class ScheduleController : ControllerBase
  {
    private readonly IDocumentService _documentService;
    private readonly IPayslipService _payslipService;
    private readonly IPayrollService _payrollService;
    private readonly IEmployeeService _employeeService;

    public ScheduleController(IDocumentService documentService, IPayslipService payslipService, IPayrollService payrollService, IEmployeeService employeeService)
    {
      _documentService = documentService;
      _payslipService = payslipService;
      _payrollService = payrollService;
      _employeeService = employeeService;
    }


    public ScheduleController(IDocumentService documentService)
    {
      _documentService = documentService;
    }

    private delegate IActionResult CreatePayroll(int m, int y);
    [HttpGet]
    [Route("check-payroll-sch")]
    public async Task<IActionResult> CheckData()
    {
      try
      {
        //lấy doc hiện hành
        var currentDoc = _documentService.Get(_ => _.Status == DocStatus.Priority, _ => _.Payrolls, _ => _.Formula.FormulaDetails);
        if (currentDoc == null)
        {
          currentDoc = _documentService.Get(_ => _.Status == DocStatus.Active, _ => _.Payrolls, _ => _.Formula.FormulaDetails);
        }
        if (currentDoc != null)
        {
          var checkAll = true;

          var now = DateTimeOffset.Now;
          var month = now.Month;
          var year = now.Year;
          if (now.Day < currentDoc.CloseDay)
          {
            --month;
            if (month == 12)
            {
              --year;
            }
          }


          if (now.Day >= currentDoc.CloseDay)
          {
            #region check data

            // kiểm tra có payroll chưa
            var checkPayroll = _payrollService.GetAll().Where(_ => _.Month == month
              && _.Year == year).ToList();

            if (checkPayroll.Count == 0)
            {
              //tạo payroll
              CreatePayroll createPayroll = new CreatePayroll(new PayrollController(_payrollService, _documentService).Create);
              createPayroll(month, year);

              // notify payroll
              #region Send Notify
              var emps = _employeeService.GetAll(_ => _.Account.Role).Where(_ => _.Account.Role.Name == "chief" || _.Account.Role.Name == "accountant").ToList();
              foreach (var emp in emps)
              {
                await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
                {
                  Role = emp.Account.Role.Name,
                  IsReading = false,
                  Status = NotiStatus.ACC_NEWPAYROLL,
                  Month = month,
                  Year = year,
                  CreatedDate = DateTimeOffset.Now
                });
              }
              #endregion
              return Ok("payroll ADD");
            }
            else
            {
              // kiểm tra payroll publish chưa
              var checkPublish = checkPayroll.FirstOrDefault(_ => _.Status == PayrollStatus.Published || _.Status == PayrollStatus.Completed);
              if (checkPublish == null)
              {
                checkAll = false;
                // notify publish payroll
                #region Send Notify
                var emps = _employeeService.GetAll(_ => _.Account.Role).Where(_ => _.Account.Role.Name == "chief" || _.Account.Role.Name == "accountant").ToList();
                foreach (var emp in emps)
                {
                  await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
                  {
                    Role = emp.Account.Role.Name,
                    IsReading = false,
                    Status = NotiStatus.ACC_DATA_MISSING,
                    CreatedDate = DateTimeOffset.Now,
                    Month = month,
                    Year = year
                  });
                }
                #endregion
              }
              else if (checkPublish.Status == PayrollStatus.Completed)
              {
                checkAll = false;
              }

              if (checkAll)
              {
                // tính lương
                // update payslip
                _payslipService.GetAll().Where(_ => _.PayrollId == checkPublish.Id).ToList().ForEach(payslip =>
                {
                  payslip.Amount = _payslipService.GetFormula(currentDoc.Formula, payslip.Id, payslip.EmpId).Value;
                });
              }


            }
            #endregion
          }
          else if (now.Day <= currentDoc.Deadline)
          {
            #region check data

            // kiểm tra có payroll chưa
            var checkPayroll = _payrollService.GetAll().Where(_ => _.Month == month
              && _.Year == year).ToList();

            if (checkPayroll.Count == 0)
            {
              //tạo payroll
              CreatePayroll createPayroll = new CreatePayroll(new PayrollController(_payrollService, _documentService).Create);
              createPayroll(month, year);

              // notify payroll
              #region Send Notify
              var emps = _employeeService.GetAll(_ => _.Account.Role).Where(_ => _.Account.Role.Name == "chief" || _.Account.Role.Name == "accountant").ToList();
              foreach (var emp in emps)
              {
                await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
                {
                  Role = emp.Account.Role.Name,
                  IsReading = false,
                  Status = NotiStatus.ACC_NEWPAYROLL,
                  Month = month,
                  Year = year,
                  CreatedDate = DateTimeOffset.Now
                });
              }
              #endregion
              return Ok("payroll ADD");
            }
            else
            {
              // kiểm tra payroll publish chưa
              var checkPublish = checkPayroll.FirstOrDefault(_ => _.Status == PayrollStatus.Published || _.Status == PayrollStatus.Completed);
              if (checkPublish == null)
              {
                checkAll = false;
                // notify publish payroll
                #region Send Notify
                var emps = _employeeService.GetAll(_ => _.Account.Role).Where(_ => _.Account.Role.Name == "chief" || _.Account.Role.Name == "accountant").ToList();
                foreach (var emp in emps)
                {
                  await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
                  {
                    Role = emp.Account.Role.Name,
                    IsReading = false,
                    Status = NotiStatus.ACC_DATA_MISSING,
                    CreatedDate = DateTimeOffset.Now,
                    Month = month,
                    Year = year
                  });
                }
                #endregion
              }
              else if (checkPublish.Status == PayrollStatus.Completed)
              {
                checkAll = false;
              }

              if (checkAll)
              {
                // tính lương
                // update payslip
                _payslipService.GetAll().Where(_ => _.PayrollId == checkPublish.Id).ToList().ForEach(payslip =>
                {
                  payslip.Amount = _payslipService.GetFormula(currentDoc.Formula, payslip.Id, payslip.EmpId).Value;
                });
              }


            }
            #endregion
          }
        }
        else return BadRequest("No doc");


        return Ok("normal day");
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpGet]
    [Route("send-email-sch")]
    public async Task<IActionResult> SendMail()
    {
      try
      {
        //lấy doc hiện hành
        var currentDoc = _documentService.Get(_ => _.Status == DocStatus.Priority, _ => _.Payrolls, _ => _.Formula.FormulaDetails);
        if (currentDoc == null)
        {
          currentDoc = _documentService.Get(_ => _.Status == DocStatus.Active, _ => _.Payrolls, _ => _.Formula.FormulaDetails);
        }
        if (currentDoc != null)
        {
          var now = DateTimeOffset.Now;
          var month = now.Month;
          var year = now.Year;
          if (now.Day < currentDoc.CloseDay)
          {
            --month;
            if (month == 12)
            {
              --year;
            }
          }

          //kiểm tra có payroll publish chưa?
          var publishPayroll = new Payroll();
          var pPayroll = currentDoc.Payrolls.FirstOrDefault(_ => _.Status == PayrollStatus.Published);
          var nPayroll = currentDoc.Payrolls.FirstOrDefault(_ => _.Status == PayrollStatus.New);

          if (pPayroll != null || nPayroll.PayDate.Value.AddDays(-1) <= now)
          {
            if (pPayroll != null)
            {
              publishPayroll = pPayroll;
            }
            else publishPayroll = nPayroll;
          }

          if (publishPayroll != null)
          {
            if (publishPayroll.PayDate.Value.AddDays(-1) <= now)
            {
              HttpClient client;
              client = new HttpClient();
              var payrollId = publishPayroll.Id;
              var uri = new Uri("http://localhost:3911/api/Mail/send-mail?payrollId=" + payrollId);
              //var uri = new Uri("http://http://spcs.azurewebsites.net/api/Mail/send-mail?payrollId=" + payrollId);
              client.BaseAddress = uri;
              await client.GetAsync(uri);

              publishPayroll.Status = PayrollStatus.Completed;
              _payrollService.Update(publishPayroll);

              ////update-payroll-status
              //client = new HttpClient();
              //uri = new Uri("http://localhost:3911/api/Schedule/update-payroll-sch");
              ////var uri = new Uri("http://http://spcs.azurewebsites.net/api/Schedule/update-payroll-sch");
              //client.BaseAddress = uri;
              //await client.GetAsync(uri);

              if (currentDoc.Status == DocStatus.Priority)
              {
                currentDoc.Status = DocStatus.Deactive;
                _documentService.Update(currentDoc);
              }
              var emps = _employeeService.GetAll(_ => _.Account.Role).ToList();
              foreach (var emp in emps)
              {
                await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
                {
                  Role = emp.Account.Role.Name,
                  IsReading = false,
                  Status = NotiStatus.EMP_NEWPAYSLIP,
                  Month = publishPayroll.Month,
                  Year = publishPayroll.Year,
                  CreatedDate = DateTimeOffset.Now
                });
              }
              return Ok("send email success");
            }
          }
          // kiểm tra ngày hiện tại == paydate
        }

        return Ok("k send");
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpGet]
    [Route("update-payroll-sch")]
    public IActionResult UpdateParoll()
    {
      try
      {
        var now = DateTimeOffset.Now;
        _payrollService.GetAll().Where(_ => (_.Status == PayrollStatus.New || _.Status == PayrollStatus.Published) && _.PayDate < now)
          .ToList().ForEach(p =>
          {
            p.Status = PayrollStatus.Expried;
            _payrollService.Update(p);
          });
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpGet]
    [Route("active-doc-sch")]
    public async Task<IActionResult> ActiveDoc()
    {
      try
      {
        var currentDoc = _documentService.Get(_ => _.Status == DocStatus.Active);
        var now = DateTimeOffset.Now;
        var doc = _documentService.GetAll().Where(_ => _.ApplyDate <= now && _.FormulaId != null)
          .OrderByDescending(_ => _.ApplyDate).ThenByDescending(_ => _.SignDate).FirstOrDefault();

        if (doc != null)
        {
          if (currentDoc != null)
          {
            currentDoc.Status = DocStatus.Deactive;
            _documentService.Update(currentDoc);
          }

          doc.Status = DocStatus.Active;
          _documentService.Update(doc);

          #region Send Notify
          var emps = _employeeService.GetAll(_ => _.Account.Role).Where(_ => _.Account.Role.Name == "chief").ToList();
          foreach (var emp in emps)
          {
            await FirebaseDatabase.GetFirebaseDatabase().sendMessage(new FireBaseVM
            {
              Role = emp.Account.Role.Name,
              IsReading = false,
              Status = NotiStatus.ACC_APPLY_DOCUMENT,
              DocCode = doc.Code,
              CreatedDate = DateTimeOffset.Now
            });
          }
          #endregion
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
