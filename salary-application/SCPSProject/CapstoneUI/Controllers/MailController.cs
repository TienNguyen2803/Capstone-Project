using BusinessLogic.Define;
using CapstoneUI.Utils;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class MailController : _BaseController
  {
    private readonly IDocumentService _documentService;
    private readonly IPayrollService _payrollService;
    private readonly IPositionDetailService _positionDetailService;
    private readonly IPayslipService _payslipService;
    private readonly IMonthlySalaryComponentService _monthlySalaryComponentService;
    private readonly IEmployeeService _employeeService;
    private readonly ISalaryComponentService _salaryComponentService;
    private readonly IFieldService _fieldService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IFormulaService _formulaService;
    private readonly IConstantTypeService _constantTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IReferenceTableDetailService _referenceTableDetailService;
    private readonly IPayslipTemplateService _payslipTemplateService;

    public MailController(
      IPayslipTemplateService payslipTemplateService,
       IDocumentService documentService,
       IReferenceTableDetailService referenceTableDetailService,
      IFormulaService formulaService,
      IPayrollService payrollService,
      IMonthlySalaryComponentService monthlySalaryComponentService,
      IEmployeeService employeeService,
      ISalaryComponentService salaryComponentService,
      IPayslipService payslipService,
      IPositionDetailService positionDetailService,
      IFieldService fieldService,
      IFieldTypeService fieldTypeService,
      IReferenceTableTypeService referenceTableTypeService,
      IConstantTypeService constantTypeService,
      IFormulaTypeService formulaTypeService
      )
    {
      _payslipTemplateService = payslipTemplateService;
      _documentService = documentService;
      _formulaService = formulaService;
      _payrollService = payrollService;
      _monthlySalaryComponentService = monthlySalaryComponentService;
      _employeeService = employeeService;
      _salaryComponentService = salaryComponentService;
      _payslipService = payslipService;
      _positionDetailService = positionDetailService;
      _fieldService = fieldService;
      _fieldTypeService = fieldTypeService;
      _constantTypeService = constantTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _formulaTypeService = formulaTypeService;
      _referenceTableDetailService = referenceTableDetailService;
    }


    [HttpGet]
    [Route("ListTempalte")]
    public IActionResult ListTempalte(string DocumentName)
    {
      try
      {
        Document document = _documentService.Get(x => x.Code.Trim() == DocumentName.Trim(), _ => _.PayslipTemplates);
        List<PaySlipTemplateAllVM> paySlips = new List<PaySlipTemplateAllVM>();
        foreach (var item in document.PayslipTemplates)
        {
          paySlips.Add(new PaySlipTemplateAllVM { Code = document.Code.Trim(), Status = item.Status, TemplateUrl = item.TemplateUrl.Split("\\")[3].Split(".")[0] });
        }
        return Ok(paySlips);
      }
      catch (Exception ex)
      {

        return StatusCode(500);
      }
    }

    [HttpGet]
    [Route("TemplateDefault")]
    public IActionResult TemplateDefault(string documentName)
    {
      try
      {
        Document document = _documentService.Get(x => x.Code.Trim() == documentName.Trim());
        var template = _payslipTemplateService.Get(x => x.DocId == document.Id && x.Status == true) ;
        return Ok(template == null ? "":  template.TemplateUrl.Split("\\")[3]);
      }
      catch (Exception ex)
      {

        return BadRequest(ex);
      }
    }


    [HttpPost]
    [Route("File")]
    public IActionResult Files(string fileName)
    {
      try
      {
        var path = Directory.GetCurrentDirectory() + @"\Resources\DocumentTemplate\";
        List<string> files = new DirectoryInfo(path).GetFiles().Select(o => o.Name).ToList();
        var pathFile = "";
        foreach (var item in files)
        {
          if (item.Split(".")[0].Equals(fileName))
          {
            pathFile = item;
          }
        }

        var memory = new MemoryStream();
        using (var stream = new FileStream(path + pathFile, FileMode.Open))
        {
          stream.CopyTo(memory);
        }
        memory.Position = 0;
        return File(memory, "application/octet-stream", pathFile);
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }

    }
    [HttpGet]
    [Route("ListFieldsMerge")]
    public IActionResult ListFieldsMerge(string code)
    {
      try
      {
        Document document = _documentService.Get(x => x.Code == code, _ => _.Formula.FormulaDetails, _ => _.Formula.FormulaTypes);
        List<string> keyValues = new List<string>();
        keyValues.Add( "<<" + document.Formula.Name.Trim().Replace(" ","_")+">>");
        keyValues = AddFields(keyValues, document.Formula);

        keyValues.Add("<<Tên_Nhân_Viên>>");
        keyValues.Add("<<Mã_Nhân_Viên>>");
        keyValues.Add("<<Email>>");
        keyValues.Add("<<Bộ_phận>>");
        keyValues.Add("<<Địa_chỉ>>");
        keyValues.Add("<<Tháng>>");
        keyValues.Add("<<Năm>>");

        return Ok(keyValues);
      }
      catch (Exception ex)
      {

        return BadRequest(ex);
      }
    }

    [HttpGet]
    [Route("ListNameDocument")]
    public IActionResult ListNameDocument()
    {
      try
      {
        return Ok(_documentService.GetAll().Select(_ => _.Code));
      }
      catch (Exception ex)
      {

        return BadRequest(ex);
      }
    }

    [HttpDelete]
    [Route("NameTemplate")]
    public IActionResult NameTemplate(string path)
    {
      try
      {
        MailUtils utils = new MailUtils();

        return Ok(utils.DeleteTemplateDocument(path));
      }
      catch (Exception ex)
      {

        return BadRequest(ex);
      }
    }


    [HttpPost]
    [Route("Export")]
    public IActionResult Export(string code)
    {
      try
      {
        byte[] fileContents;
        // Get you IEnumerable<T> data
        Document document = _documentService.Get(x => x.Code == code, _ => _.Formula.FormulaDetails, _ => _.Formula.FormulaTypes);
        List<string> keyValues = new List<string>();
        keyValues.Add(document.Formula.Name);
        keyValues = AddFields(keyValues, document.Formula);

        keyValues.Add("<<Tên_Nhân_Viên>>");
        keyValues.Add("<<Mã_Nhân_Viên>>");
        keyValues.Add("<<Email>>");
        keyValues.Add("<<Bộ_phận>>");
        keyValues.Add("<<Địa_chỉ>>");
        keyValues.Add("<<Tháng>>");
        keyValues.Add("<<Năm>>");
        //string sWebRootFolder = _hostingEnvironment.WebRootPath;
        string sFileName = @"Field.xlsx";
        //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
        FileInfo file = new FileInfo(Path.Combine(@"Utils", sFileName));
        if (file.Exists)
        {
          file.Delete();
          file = new FileInfo(Path.Combine(@"Utils", sFileName));
        }
        using (ExcelPackage package = new ExcelPackage(file))
        {
          // add a new worksheet to the empty workbook
          ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
          //First add the headers
          int i = 1;
          foreach (var item in keyValues)
          {
            worksheet.Cells[1, i++].Value = item;
          }


          //package.Save(); //Save the workbook.
          fileContents = package.GetAsByteArray();

        }
        return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
      }
      catch (Exception e)
      {

        return Ok(e);
      }

    }
    [HttpGet]
    [Route("send-mail")]
    public IActionResult SendMailMerge(int payrollId)
    {
      try
      {
        MailUtils utils = new MailUtils();
        //var PayrollId = _payrollService.Get(x => x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year, _ => _.Payslips);
        //var temp = DateTimeOffset.Now.Month;
        var Payroll = _payrollService.Get(x => x.Id == payrollId, _ => _.Payslips);
        foreach (var item in Payroll.Payslips)
        {
          Dictionary<string, string> keyValues = new Dictionary<string, string>();
          // 1 field 2 ref 3 formula 4 constant
          var result = _payslipService.Get(_ => _.EmpId == item.EmpId && _.PayrollId == item.PayrollId
        , _ => _.Employee, _ => _.Payroll.Document.Formula.FormulaDetails, _ => _.Employee.Department, _ => _.Employee.PositionDetails
          );

          var mainFormula = _payslipService.GetFormula(result.Payroll.Document.Formula, item.Id, result.Employee.Code);

          PaySlipMailVM output = new PaySlipMailVM();
          PayrollMailVM payrollVM = new PayrollMailVM();
          output.EmpName = result.Employee.Fullname;
          output.Department = result.Employee.Department.DepName;
          output.Address = result.Employee.Address;
          output.Email = result.Employee.Email;
          payrollVM.Month = result.Payroll.Month;
          payrollVM.Year = result.Payroll.Year;
          payrollVM.formula = mainFormula;
          output.payrollVM = payrollVM;

          keyValues.Add("&lt;&lt;Mã_Nhân_Viên&gt;&gt;", result.Employee.Code);
          keyValues.Add("&lt;&lt;Tên_Nhân_Viên&gt;&gt;", output.EmpName);
          keyValues.Add("&lt;&lt;Email&gt;&gt;", output.Email);
          keyValues.Add("&lt;&lt;Bộ_phận&gt;&gt;", output.Department);
          keyValues.Add("&lt;&lt;Địa_chỉ&gt;&gt;", output.Address);
          keyValues.Add("&lt;&lt;Tháng&gt;&gt;", output.payrollVM.Month.ToString());
          keyValues.Add("&lt;&lt;Năm&gt;&gt;", output.payrollVM.Year.ToString());
          keyValues.Add("&lt;&lt;" + output.payrollVM.formula.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", output.payrollVM.formula.Value.ToString());
          foreach (var field in output.payrollVM.formula.Details)
          {
            switch (field.Type)
            {
              case 1:
                if (keyValues.ContainsKey("&lt;&lt;" + field.FieldType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
                {
                  break;
                }
                else
                {
                  keyValues.Add("&lt;&lt;" + field.FieldType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.FieldType.Value);
                }
                break;
              case 2:
                if (keyValues.ContainsKey("&lt;&lt;" + field.RefType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
                {
                  break;
                }
                else
                {
                  keyValues.Add("&lt;&lt;" + field.RefType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.RefType.Value);
                }
                break;
              case 3:
                if (keyValues.ContainsKey("&lt;&lt;" + field.FormulaType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
                {
                  break;
                }
                else
                {
                  keyValues.Add("&lt;&lt;" + field.FormulaType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.FormulaType.Value.ToString());
                  BusinessLogic.Implement.FormulaTypeSSVM formula = field.FormulaType;
                  keyValues = utils.AddValue(keyValues, formula);
                }
                break;
              case 4:
                break;
            }
          }
          List<string> fieldNames = new List<string>(keyValues.Keys);
          List<string> fieldValues = new List<string>(keyValues.Values);
     
          //get template email
          var template = _payslipTemplateService.Get(x => x.Status == true && x.DocId == Payroll.DocId).TemplateUrl.Split("\\")[3];


          if (template.Split(".")[1].Equals("html"))
          {
            utils.SendMailHtml(keyValues, template, result.Employee.Email,keyValues["&lt;&lt;Tháng&gt;&gt;"],keyValues["&lt;&lt;Năm&gt;&gt;"]);
          }
          else
          {
            utils.OpenWord(fieldNames.ToArray(), fieldValues.ToArray(), result.Employee.Email, template);
          }
          keyValues.Clear();
        }
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }


    [HttpGet]
    [Route("Data-demo")]
    public IActionResult DataDemo(string Code, string template)
    {
      try
      {
        MailUtils utils = new MailUtils();
        Document document = _documentService.Get(x => x.Code == Code, _ => _.Payrolls, _ => _.Formula.FormulaDetails);
        Dictionary<string, string> keyValues = new Dictionary<string, string>();

        //var payroll = _payslipService.Get(x => x.PayrollId == document.Payrolls.First().Id);
        //if (document.Payrolls.Count > 0)
        if (true)
        {
          Employee employee = _employeeService.GetAll(_ => _.Department).FirstOrDefault();
          keyValues = new Dictionary<string, string>();

          //1 field 2 ref 3 formula 4 constant

          //var mainFormula = _payslipService.GetFormula(result.Payroll.Document.Formula, payroll.Id, result.Employee.Code);
          var mainFormula = GetFormula(document.Formula);

          PaySlipMailVM output = new PaySlipMailVM();
          PayrollMailVM payrollVM = new PayrollMailVM();

          //add by cindy
          var month = DateTime.Now.Month;
          var year = DateTime.Now.Year;

          output.EmpName = employee.Fullname;
          output.Department = employee.Department.DepName;
          output.Address = employee.Address;
          output.Email = employee.Email;
          payrollVM.Month = month;
          payrollVM.Year = year;
          payrollVM.Formula = mainFormula;
          output.payrollVM = payrollVM;

          keyValues.Add("&lt;&lt;Mã_Nhân_Viên&gt;&gt;", employee.Code);
          keyValues.Add("&lt;&lt;Tên_Nhân_Viên&gt;&gt;", output.EmpName);
          keyValues.Add("&lt;&lt;Email&gt;&gt;", output.Email);
          keyValues.Add("&lt;&lt;Bộ_phận&gt;&gt;", output.Department);
          keyValues.Add("&lt;&lt;Địa_chỉ&gt;&gt;", output.Address);
          keyValues.Add("&lt;&lt;Tháng&gt;&gt;", output.payrollVM.Month.ToString());
          keyValues.Add("&lt;&lt;Năm&gt;&gt;", output.payrollVM.Year.ToString());
          keyValues.Add("&lt;&lt;" + output.payrollVM.Formula.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", output.payrollVM.Formula.Value.ToString());
          foreach (var field in output.payrollVM.Formula.Details)
          {
            switch (field.Type)
            {
              case 1:
                if (keyValues.ContainsKey( "&lt;&lt;"+field.FieldType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
                {
                  break;
                }
                else
                {
                  keyValues.Add("&lt;&lt;" + field.FieldType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.FieldType.Value);
                }
                break;
              case 2:
                if (keyValues.ContainsKey("&lt;&lt;" + field.RefType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
                {
                  break;
                }
                else
                {
                  keyValues.Add("&lt;&lt;" + field.RefType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.RefType.Value);
                }
                break;
              case 3:
                if (keyValues.ContainsKey("&lt;&lt;" + field.FormulaType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;"))
                {
                  break;
                }
                else
                {
                  keyValues.Add("&lt;&lt;" + field.FormulaType.Name.Trim().Replace(" ", "_")+ "&gt;&gt;", field.FormulaType.Value.ToString());
                  FormulaTypeSSVM formula = field.FormulaType;
                  keyValues = utils.AddValue(keyValues, formula);
                }
                break;
              case 4:
                break;
            }
          }
          utils.ReviewDataTemplate(keyValues, template);
        }
        else
        {
          return BadRequest("false");
        }
        return Ok(utils.ReviewDataTemplate(keyValues, template));
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }

    private FormulaTypeSSVM GetFormula(Formula formula)
    {
      FormulaTypeSSVM result = new FormulaTypeSSVM();

      result.Id = formula.Id;
      result.Name = formula.Name;
      result.Details = new List<FormulaDetailSSVM>();
      string left = result.Name + " = ";
      string strFormular = left;
      formula.FormulaDetails.ToList().ForEach(fDetail =>
      {
        FormulaDetailSSVM detail = new FormulaDetailSSVM();
        detail.Id = fDetail.Id;
        detail.Operator = fDetail.Operator;
        detail.Ordinal = fDetail.Ordinal;
        detail.Type = fDetail.Type;

        switch (fDetail.Type)
        {
          case 1:

            var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
            var field = _fieldService.Get(_ => _.Id == fieldType.FieldId);
            detail.FieldType = new RefTypeSSVM();
            detail.FieldType.Name = field.Name;
            detail.FieldType.Value = field.SampleValue;
            break;
          case 2:
            var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.ReferenceTable.ReferenceTableDetails);

            //var field2 = _fieldService.Get(_ => _.Id == refType.ReferenceTable.SourceValue);
            //var refef = _referenceTableDetailService.Get(_ => _.Key == field2.Value);
            var refef = _referenceTableDetailService.Get(_ => _.ReferenceTableId == refType.RefenceTableTypeId);
            if (refef != null)
            {
              detail.RefType = new RefTypeSSVM();
              detail.RefType.Name = refType.ReferenceTable.Name;
              detail.RefType.Value = refef.Value;
            }
            else
            {
              detail.RefType = new RefTypeSSVM();
              detail.RefType.Name = refType.ReferenceTable.Name;
              detail.RefType.Value = "0";
            }

            break;
          case 3:
            var formular = _formulaTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.Formula.FormulaDetails);
            detail.FormulaType = GetFormula(formular.Formula);
            break;
          case 4:
            var constantType = _constantTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
            detail.ConstantType = new ConstantTypeSSVM();
            detail.ConstantType.Value = constantType.Value; // ec
            break;
        }

        result.Details.Add(detail);
      });

      // calculate value

        string expression = "0";

            result.Details.ToList().ForEach(_ =>
            {
                expression += " ";

                switch (_.Type)
                {
                    case 1:
                        expression += _.Operator.ToOperator();
                        expression += " " + _.FieldType.Value;
                        strFormular += _.Operator.ToOperator() + _.FieldType.Name;
                        _.Value = _.FieldType.Value;
                        break;
                    case 2:
                        expression += _.Operator.ToOperator();
                        expression += " " + _.RefType.Value;
                        strFormular += _.Operator.ToOperator() + _.RefType.Name;
                        _.Value = _.RefType.Value;
                        break;
                    case 3:
                        expression += _.Operator.ToOperator();
                        expression += " " + _.FormulaType.Value;
                        strFormular += _.Operator.ToOperator() + _.FormulaType.Name;
                        _.Value = _.FormulaType.Value.ToString();
                        break;
                    case 4:
                        expression += _.Operator.ToOperator();
                        expression += " " + (_.ConstantType.Value);
                        strFormular += _.Operator.ToOperator() + _.ConstantType.Value;
                        _.Value = _.ConstantType.Value.ToString();
                        break;
                }
                strFormular += " ";

            });
            strFormular = strFormular.Replace(left + result.Details.FirstOrDefault().Type.ToOperator(), left);
            strFormular = strFormular.Replace("= +", "=");
            switch (formula.Type)
            {
                case 1:
                    result.Value = CalculateUtil.evaluate(expression);
                    result.Formula = strFormular;
                    break;
                case 2:
                    result.Value = Decimal.Parse(result.Details.Min(_ => _.Value));
                    result.Formula = result.Name;
                    break;
                case 3:
                    result.Value = Decimal.Parse(result.Details.Max(_ => _.Value));
                    result.Formula = result.Name;
                    break;
                case 4:
                    result.Value = result.Details.Average(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 5:
                    result.Value = Decimal.Parse(result.Details.Max(_ => _.Value)) - Decimal.Parse(result.Details.Min(_ => _.Value));
                    result.Formula = result.Name;
                    break;
            }

            return result;
    }

    [HttpPost]
    [Route("UpLoad")]
    public IActionResult Upload([FromBody] MailVM template)
    {

      try
      {
        MailUtils utils = new MailUtils();
        utils.Upload(template.Base64, template.FileName);
        return Ok();
      }
      catch (Exception e)
      {

        return BadRequest(e);
      }

    }
    [HttpGet]
    [Route("StringtoHtml")]
    public IActionResult StringtoHtml(string html)
    {

      try
      {
        MailUtils utils = new MailUtils();
        utils.HtmlString(html);
        return Ok();
      }
      catch (Exception e)
      {

        return BadRequest(e);
      }

    }
    // POST api/values
    [HttpPost, DisableRequestSizeLimit]
    [Route("UploadTemplate")]
    public IActionResult Post()
    {
      try
      {
        MailUtils utils = new MailUtils();
        //get file and template from form data .
        var file = Request.Form.Files[0];
        var template = Request.Form["template"];
        // khai báo Path to save
        var fullPath = Directory.GetCurrentDirectory();
        bool flag = true;
        if (file.Length > 0)
        {
          //get Name File
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          var dbPath = @"\Resources\DocumentTemplate\" + fileName;
          var Path = fullPath + dbPath;
          PayslipTemplate tem = new PayslipTemplate();
          // khai báo templateDoc để kiểm tra , tồn tại thì xóa ,ngược lại lưu
          Document doc = _documentService.Get(x => x.Code == template, _ => _.PayslipTemplates);
          if (doc.PayslipTemplates.Count == 0)
          {
            tem = new PayslipTemplate();
            tem.Status = true;
            tem.TemplateUrl = dbPath;
            tem.DocId = doc.Id;
            _payslipTemplateService.Create(tem);
            flag = false;
          }
          else
          {
            foreach (var item in doc.PayslipTemplates)
            {
              if (item.TemplateUrl.Equals(dbPath.Trim().Replace(".html", ".docx")))
              {
                utils.DeleteImage(Path.Trim().Replace(".html", ".docx"));
                item.TemplateUrl = dbPath;
                item.Status = true;
                _payslipTemplateService.Update(item);
                flag = false;
              }
              else if (item.TemplateUrl.Equals(dbPath))
              {
                utils.DeleteImage(Path);
                item.TemplateUrl = dbPath;
                item.Status = true;
                _payslipTemplateService.Update(item);
                flag = false;
              }
              else
              {
                item.Status = false;
                _payslipTemplateService.Update(item);
              }
            }

          }
          if (flag)
          {
            PayslipTemplate temp = new PayslipTemplate();
            temp.Status = true;
            temp.TemplateUrl = dbPath;
            temp.DocId = doc.Id;
            _payslipTemplateService.Create(temp);
          }

          using (var stream = new FileStream(Path, FileMode.Create))
          {
            file.CopyTo(stream);
          }

        }

        return Ok();

      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }

    }
    // POST api/values
    [HttpPost, DisableRequestSizeLimit]
    [Route("UploadTemplateV2")]
    public IActionResult UploadV2()
    {
      try
      {
        MailUtils utils = new MailUtils();
        //get file and template from form data .
        var file = Request.Form.Files[0];
        var template = Request.Form["template"];
        // khai báo Path to save
        var fullPath = Directory.GetCurrentDirectory();

        if (file.Length > 0)
        {
          //get Name File
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          var dbPath = @"\Resources\DocumentTemplate\" + fileName;
          var Path = fullPath + dbPath;

          // khai báo templateDoc để kiểm tra , tồn tại thì xóa ,ngược lại lưu
          Document doc = _documentService.Get(x => x.Code == template, _ => _.PayslipTemplates);

          if (doc.PayslipTemplates.Count != 0)
          {
            foreach (var item in doc.PayslipTemplates)
            {
              if (item.TemplateUrl.Equals(dbPath))
              {
                return BadRequest("Template is exist !");
              }
            }

          }

          PayslipTemplate tem = new PayslipTemplate();
          tem.Status = false;
          tem.TemplateUrl = dbPath;
          tem.DocId = doc.Id;
          _payslipTemplateService.Create(tem);
          using (var stream = new FileStream(Path, FileMode.Create))
          {
            file.CopyTo(stream);
          }

          return Ok();
        }
        else
        {
          return BadRequest("Không có file  ");
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }

    }
    private List<string> AddFields(List<string> list, Formula formula)
    {
      foreach (var item in formula.FormulaDetails)
      {
        switch (item.Type)
        {
          case 1:
            DataAccess.Entities.FieldType fieldType = _fieldTypeService.Get(x => x.FormulaDetailId == item.Id, _ => _.Field);
            if (list.Contains( "<<" + fieldType.Field.Name.Trim().Replace(" ", "_")+">>"))
            {
              break;
            }
            else
            {
              list.Add("<<"+fieldType.Field.Name.Trim().Replace(" ", "_")+">>");
            }
            break;
          case 2:
            ReferenceTableType refType = _referenceTableTypeService.Get(x => x.FormulaDetailId == item.Id, _ => _.ReferenceTable);
            if (list.Contains("<<" + refType.ReferenceTable.Name.Trim().Replace(" ", "_")+">>"))
            {
              break;
            }
            else
            {
              list.Add( "<<"+ refType.ReferenceTable.Name.Trim().Replace(" ", "_")+">>");
            }
            break;
          case 3:
            FormulaType formulaType = _formulaTypeService.Get(x => x.FormulaDetailId == item.Id, _ => _.Formula.FormulaDetails);
            if (list.Contains("<<" +  formulaType.Formula.Name.Trim().Replace(" ", "_")+">>"))
            {
              break;
            }
            else
            {
              list.Add("<<" + formulaType.Formula.Name.Trim().Replace(" ", "_") + ">>");
              Formula formulas = formulaType.Formula;
              list = AddFields(list, formulas);
            }
            break;
          case 4:
            break;
        }

      }
      return list;
    }
    [HttpGet]
    [Route("StringHtml")]
    public IActionResult getStringHtml(string fileName)
    {

      try
      {
        MailUtils utils = new MailUtils();

        return Ok(utils.HtmlString(fileName));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }
    [HttpGet]
    [Route("SetStatus")]
    public IActionResult SetStatus(string fileName, string code)
    {

      try
      {
        Document doc = _documentService.Get(x => x.Code == code, _ => _.PayslipTemplates);
        PayslipTemplate payslip = new PayslipTemplate();
        foreach (var item in doc.PayslipTemplates)
        {
          if (item.TemplateUrl.Split("\\")[3].Split(".")[0].Equals(fileName))
          {
            payslip = item;
          }
          else
          {
            item.Status = false;
            _payslipTemplateService.Update(item);
          }
        }
        payslip.Status = true;
        _payslipTemplateService.Update(payslip);
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [HttpPost, DisableRequestSizeLimit]
    [Route("UploadTemplateV3")]
    public IActionResult UploadV3()
    {
      try
      {
        MailUtils utils = new MailUtils();
        //get file and template from form data .
        var file = Request.Form.Files[0];
        var template = Request.Form["template"];
        // khai báo Path to save
        var fullPath = Directory.GetCurrentDirectory();

        if (file.Length > 0)
        {
          //get Name File
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          var dbPath = @"\Resources\DocumentTemplate\" + fileName;
          var Path = fullPath + dbPath;

          // khai báo templateDoc để kiểm tra , tồn tại thì xóa ,ngược lại lưu
          Document doc = _documentService.Get(x => x.Code == template, _ => _.PayslipTemplates);
          PayslipTemplate tem = new PayslipTemplate();
          if (doc.PayslipTemplates.Count != 0)
          {
            foreach (var item in doc.PayslipTemplates)
            {
              if (item.TemplateUrl.Equals(dbPath.Trim().Replace(".html", ".docx")))
              {
                utils.DeleteImage(Path.Trim().Replace(".html", ".docx"));
                item.TemplateUrl = dbPath;
                item.Status = false;
                _payslipTemplateService.Update(item);
              }
              if (item.TemplateUrl.Equals(dbPath))
              {
                utils.DeleteImage(Path);
              }
            }

          }



          using (var stream = new FileStream(Path, FileMode.Create))
          {
            file.CopyTo(stream);
          }

          return Ok();
        }
        else
        {
          return BadRequest("Không có file  ");
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }
    [HttpGet]
    [Route("PaysplitTemplate")]
    public IActionResult PaysplitTemplate()
    {
      try
      {
        var list = _documentService.GetAll(_ => _.PayslipTemplates).Where(x => x.Status == DocStatus.Active).ToList();
        var docDeactive = _documentService.GetAll(_ => _.PayslipTemplates).
          Where(x => x.Status == DocStatus.Deactive || x.Status == DocStatus.Priority).ToList();
        if (list.Count != 0)
        {
          list.AddRange(docDeactive);
        } else
        {
          list = docDeactive;
        }

        int id = 1;
        var pays = new List<PaySlipTemplateAllVM>();
        foreach (var item in list)
        {
          var payslipTemplates = item.PayslipTemplates.OrderByDescending(_ => _.Status).ToList();
          foreach (var x in payslipTemplates)
          {
            pays.Add(new PaySlipTemplateAllVM { Id = id++, Code = item.Code, Status = x.Status, TemplateUrl = x.TemplateUrl.Split("\\")[3].Split(".")[0] });
          }

        }
        pays.OrderByDescending(_ => _.Status);
        return Ok(pays);
      }
      catch (Exception)
      {

        return StatusCode(500);
      }

    }

    [HttpGet]
    [Route("PayrollDocument")]
    public IActionResult PayrollDocument(string code)
    {
      try
      {
        Document document = _documentService.Get(x => x.Code == code, _ => _.Formula);
        if (document.FormulaId == null)
        {
          return Ok("False");
        }
        return Ok("True");
      }
      catch (Exception)
      {

        return StatusCode(500);
      }

    }
  }
}
