using BusinessLogic.Define;
using CapstoneUI.Utils;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class DocumentController : _BaseController
  {
    private readonly IDocumentService _documentService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IConstantTypeService _constantTypeService;
    private readonly IFieldService _fieldService;
    private readonly IPayrollService _payrollService;
    private readonly IReferenceTableDetailService _referenceTableDetailService;
    private readonly IReferenceTableService _referenceTableService;
    private readonly IFormulaService _formulaService;
    private readonly IFormulaDetailService _formulaDetailService;

    public DocumentController(IDocumentService documentService, IConstantTypeService constantTypeService,
      IFormulaTypeService formulaTypeService, IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService,
      IFieldService fieldService, IPayrollService payrollService, IReferenceTableService referenceTableService, IFormulaService formulaService,
      IReferenceTableDetailService referenceTableDetailService, IFormulaDetailService formulaDetailService)
    {
      _documentService = documentService;
      _constantTypeService = constantTypeService;
      _formulaTypeService = formulaTypeService;
      _fieldTypeService = fieldTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _fieldService = fieldService;
      _referenceTableDetailService = referenceTableDetailService;
      _payrollService = payrollService;
      _referenceTableService = referenceTableService;
      _formulaService = formulaService;
      _formulaDetailService = formulaDetailService;
    }
    public DocumentController(IDocumentService documentService)
    {
      _documentService = documentService;
    }


    private delegate IActionResult ActiveDoc();

    [HttpPost]
    [Route("document")]
    public IActionResult Post()
    {
      try
      {
        //get file and template from form data .
        var file = Request.Form.Files[0];
        var Code = Request.Form["Code"];
        var SignDate = Request.Form["SignDate"];
        var ApplyDate = Request.Form["ApplyDate"];
        var EndDate = Request.Form["ApplyDate"];
        var CloseDay = Int32.Parse(Request.Form["CloseDay"]);
        var Deadline = Int32.Parse(Request.Form["Deadline"]);
        var Description = Request.Form["Description"];

        Document document = null;

        //var folderName = Path.Combine("Resources", "ImageDocument");
        //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        Document doc = _documentService.Get(x => x.Code == Code);
        string message = "";

        if (doc != null)
        {
          message = "Document Code is Existed";
        }
        else if (DateTimeOffset.Parse(SignDate).Date > DateTime.Now.Date)
        {
          message = "SignDate invalid";
        }
        else if (DateTimeOffset.Parse(ApplyDate).Date < DateTime.Now.Date)
        {
          message = "ApplyDate invalid";
        }
        else if (CloseDay < 1 || CloseDay > 30)
        {
          message = "CloseDay out of range 1 to 30 ";
        }
        else if (Deadline < 1 || Deadline > 30)
        {
          message = "Deadline out of range 1 to 30 ";
        }
        else
        {
          if (file != null)
          {

            document = new Document();
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var pathToSave = Directory.GetCurrentDirectory() + "/wwwroot/";
            var dbPath = "assets/ImageDocument/" + fileName;
            var fullPath = pathToSave + dbPath;
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
              file.CopyTo(stream);
            }
            document.DocumentUrl = dbPath;
          }
          else
          {
            document.DocumentUrl = null;
          }

          document.Status = DocStatus.Deactive;
          document.Code = Code;
          document.SignDate = DateTimeOffset.Parse(SignDate);
          document.ApplyDate = DateTimeOffset.Parse(ApplyDate);
          document.EndDate = DateTimeOffset.Parse(EndDate);
          document.Deadline = Deadline;
          document.CloseDay = CloseDay;
          document.Description = Description;

          var currentDoc = _documentService.Get(_ => _.Status == DocStatus.Active);
          //var currentPayroll = _payrollService.Get(_ => _.)
          var day = document.ApplyDate.Value.Day;
          var month = document.ApplyDate.Value.Month;
          var year = document.ApplyDate.Value.Year;

          _documentService.Create(document);

          var payroll = new PayrollReturnVM
          {
          };

          var isConflict = false;
          if (currentDoc != null)
          {
            if (currentDoc.CloseDay <= currentDoc.Deadline)
            {
              if (day >= currentDoc.CloseDay && day <= currentDoc.Deadline)
              {
                isConflict = true;
              }
            }
            else if (currentDoc.CloseDay > currentDoc.Deadline)
            {
              if (day >= currentDoc.CloseDay)
              {
                isConflict = true;
              }
              else if (day <= currentDoc.Deadline)
              {
                isConflict = true;
                month = month - 1;
              }
            }
            if (isConflict)
            {

              payroll.month = month;
              if (month == 12)
              {
                --year;
              }
              payroll.year = year;

              var checkPayroll = _payrollService.Get(_ => _.Month == month && _.Year == year);
              if (checkPayroll != null)
              {
                if (checkPayroll.Status == PayrollStatus.New || checkPayroll.Status == PayrollStatus.Published)
                {
                  return StatusCode(409, payroll);
                }
                return Ok(document);
              }
              return StatusCode(409, payroll);
            }
            else
              return Ok(document);
          }
          else
          {
            return Ok(document);
          }
        }
        return BadRequest(message);
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }

    [HttpPost]
    [Route("document-create")]
    public string Create(DocumentCreateVM viewModel)
    {
      try
      {
        Document document = new Document();
        document.ApplyDate = viewModel.ApplyDate;
        document.CloseDay = viewModel.CloseDay;
        document.Code = viewModel.Code;
        document.Deadline = viewModel.Deadline;
        document.Description = viewModel.Description;
        document.SignDate = viewModel.SignDate;
        document.DocumentUrl = viewModel.DocumentUrl;

        Document doc = _documentService.Get(x => x.Code == viewModel.Code);
        string message = "";

        if (doc != null)
        {
          message = "Số hiệu quyết định đã tồn tại";
        }
        else if (viewModel.SignDate.Date > DateTime.Now.Date)
        {
          message = "Ngày ký không phù hợp";
        }
        else if (viewModel.ApplyDate.Date < DateTime.Now.Date)
        {
          message = "Ngày áp dụng không phù hợp";
        }
        else if (viewModel.CloseDay < 1 || viewModel.CloseDay > 30)
        {
          message = "Ngày tính lương từ 1-30";
        }
        else if (viewModel.Deadline < 1 || viewModel.Deadline > 30)
        {
          message = "Hạn tính lương từ 1-30";
        }
        else
        {
          message = "ok";
          _documentService.Create(document);
        }
        return message;
      }
      catch (Exception e)
      {
        //var 
        throw;
      }
    }

    [HttpGet]
    [Route("ImageDocument")]
    public IActionResult ImageDocument(string code)
    {
      try
      {
        Document document = _documentService.Get(x => x.Code == code);
        string json = "{\"DocumentUrl\": ";
        var host = "\"http://" + HttpContext.Request.Host.Value + "/" + document.DocumentUrl;
        json += host + "\"}";
        return Ok(json);
      }
      catch (Exception)
      {

        return BadRequest("Code ko tồn tại ");
      }

    }
    //[HttpPost]
    //[Route("document")]
    //public IActionResult Create([FromBody] DocumentCreateVM viewModel)
    //{
    //  try
    //  {
    //    _documentService.Create(ModelMapper.ConvertToModel(viewModel));
    //    return Ok(ModelMapper.ConvertToViewModel(_documentService.GetAll().LastOrDefault()));
    //  }
    //  catch (Exception e)
    //  {
    //    return StatusCode(500, e);
    //  }
    //}

    [HttpGet]
    [Route("root-path")]
    public IActionResult GetRootPath()
    {
      try
      {
        return Content(PathUtil.rootPath);
      }
      catch (Exception)
      {

        throw;
      }
    }

    [HttpGet]
    [Route("documents")]
    public async Task<IActionResult> GetAll()
    {
      try
      {
        var userId = User.FindFirst(ClaimTypes.Name);
        var roleName = User.FindFirst(ClaimTypes.Role);
        var documents = _documentService.GetAll(_ => _.Formula.FormulaDetails).OrderByDescending(_ => _.ApplyDate).ToList();
        var output = new List<Object>();
        var Formula = new Object();
        var FormulaVM = new Object();
        foreach (var doc in documents)
        {
          if (doc.FormulaId != null)
          {
            Formula = GetFormula(doc.Formula).Formula;
            FormulaVM = GetFormulaCL(doc.Formula);
          }
          output.Add(new
          {
            Id = doc.Id,
            Code = doc.Code,
            ApplyDate = doc.ApplyDate,
            SignDate = doc.SignDate,
            Deadline = doc.Deadline,
            CloseDay = doc.CloseDay,
            Description = doc.Description,
            FormulaId = doc.FormulaId,
            DocumentUrl = doc.DocumentUrl,
            Status = doc.Status,
            EndDate = doc.EndDate,
            Formula = Formula,
            FormulaVM = FormulaVM
          });
        }
        #region active doc
        ///api/Schedule/check-payroll-sch
        var client = new HttpClient();
        var uri = new Uri("http://localhost:3911/api/Schedule/active-doc-sch");
        //var uri = new Uri("http://http://spcs.azurewebsites.net/Schedule/active-doc-sch");
        client.BaseAddress = uri;
        await client.GetAsync(uri);
        #endregion

        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    private Object GetFormulaCL(Formula formula)
    {
      var fdetail = new List<Object>();
      var formulaDetails = _formulaDetailService.GetAll(_ => _.ConstantType, _ => _.FieldType, _ => _.FormulaType, _ => _.ReferenceTableType)
        .Where(_ => _.FormulaId == formula.Id).ToList();
      foreach (var detail in formulaDetails)
      {
        switch (detail.Type)
        {
          case 1:
            var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == detail.Id, _ => _.Field);
            fdetail.Add(new
            {
              Operator = detail.Operator,
              Type = detail.Type,
              FieldTypeVM = new
              {
                Id = fieldType.FieldId,
                Name = fieldType.Field.Name
              }
            });
            break;
          case 2:
            var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == detail.Id, _ => _.ReferenceTable.ReferenceTableDetails);
            fdetail.Add(new
            {
              Operator = detail.Operator,
              Type = detail.Type,
              RefTableTypeVM = GetReftableCL(refType.ReferenceTable)
            });
            break;
          case 3:
            var formulaType = _formulaTypeService.Get(_ => _.FormulaDetailId == detail.Id, _ => _.Formula.FormulaDetails);
            fdetail.Add(new
            {
              Operator = detail.Operator,
              Type = detail.Type,
              FormulaTypeVM = GetFormulaCL(formulaType.Formula)
            });
            break;
          case 4:
            fdetail.Add(new
            {
              Operator = detail.Operator,
              Type = detail.Type,
              ConstantType = new { Value = detail.ConstantType.Value }
            });
            break;
          default:
            break;
        }
      }
      var result = new
      {
        Name = formula.Name,
        Type = formula.Type,
        FormulaDetails = fdetail
      };
      return result;
    }
    private Object GetReftableCL(ReferenceTable referenceTable)
    {
      var result = new RefTableCLVM
      {
        Id = referenceTable.Id,
        Name = referenceTable.Name,
        CompareType = referenceTable.CompareType,
        SourceType = referenceTable.SourceType,
        SourceValue = referenceTable.SourceValue
      };

      switch (referenceTable.SourceType)
      {
        case 1:
          var field = _fieldService.Get(_ => _.Id == referenceTable.SourceValue);
          result.SourceName = field.Name;
          result.FieldVM = new
          {
            Id = field.Id,
            Name = field.Name
          };
          break;
        case 2:
          var refTable = _referenceTableService.Get(_ => _.Id == referenceTable.SourceValue, _ => _.ReferenceTableDetails, _ => _.ReferenceTableTypes);
          result.SourceName = refTable.Name;
          result.RefTableVM = GetReftableCL(refTable);
          break;
        case 3:
          var formula = _formulaService.Get(_ => _.Id == referenceTable.SourceValue, _ => _.FormulaDetails, _ => _.FormulaTypes);
          result.SourceName = formula.Name;
          result.FormulaVM = GetFormulaCL(formula);
          break;
        default:
          break;
      }

      var refDetails = new List<Object>();
      foreach (var rd in referenceTable.ReferenceTableDetails)
      {
        switch (referenceTable.ReturnType)
        {
          case "1":
            var field = _fieldService.Get(_ => _.Name == rd.Value);
            refDetails.Add(new
            {
              Key = rd.Key,
              Value = rd.Value,
              FieldVM = new
              {
                Id = field.Id,
                Name = field.Name
              }
            });
            break;
          case "2":
            var reftable = _referenceTableService.Get(_ => _.Name == rd.Value);
            refDetails.Add(new
            {
              Key = rd.Key,
              Value = rd.Value,
              RefTableVM = GetReftableCL(reftable)
            });
            break;
          case "3":
            var formula = _formulaService.Get(_ => _.Name == rd.Value);
            refDetails.Add(new
            {
              Key = rd.Key,
              Value = rd.Value,
              FormulaVM = GetFormulaCL(formula)
            });
            break;
          default:
            refDetails.Add(new
            {
              Key = rd.Key,
              Value = rd.Value,
            });
            break;
        }

      }
      result.RefTableDetails = refDetails;
      return result;
    }

    [HttpGet]
    [Route("document")]
    public IActionResult Get(int id)
    {
      try
      {
        var result = _documentService.Get(_ => _.Id == id, _ => _.Formula.FormulaDetails);
        var output = ModelMapper.ConvertToViewModel(result);
        if (result.FormulaId != null)
        {
          output.FormulaId = result.FormulaId;
          output.FormulaVM = ModelMapper.ConvertToViewModel(result.Formula);
          output.Formula = GetFormula(result.Formula).Formula;
        }
        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    //[HttpPut]
    //[Route("document")]
    //public IActionResult Mapping(DocumentUpdateVM viewModel)
    //{
    //  try
    //  {
    //    var model = _documentService.Get(_ => _.Id == viewModel.Id);
    //    model.FormulaId = viewModel.FormulaId;
    //    _documentService.Update(model);

    //    return Ok();
    //  }
    //  catch (Exception e)
    //  {
    //    return StatusCode(500, e);
    //  }

    //}

    [HttpPut]
    [Route("document")]
    public IActionResult Update()
    {
      try
      {
        //get file and template from form data .
        var file = Request.Form.Files[0];
        var Code = Request.Form["Code"];
        var SignDate = Request.Form["SignDate"];
        var ApplyDate = Request.Form["ApplyDate"];
        var CloseDay = Int32.Parse(Request.Form["CloseDay"]);
        var Deadline = Int32.Parse(Request.Form["Deadline"]);
        var Description = Request.Form["Description"];
        var FormulaId = Request.Form["FormulaId"];


        Document model = _documentService.Get(_ => _.Code == Code);
        model.Code = Code;
        model.SignDate = DateTimeOffset.Parse(SignDate);
        model.ApplyDate = DateTimeOffset.Parse(ApplyDate);
        model.CloseDay = CloseDay;
        model.FormulaId = Int32.Parse(FormulaId);
        model.Deadline = Deadline;
        model.Description = Description;


        if (file != null)
        {
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          var pathToSave = Directory.GetCurrentDirectory() + "/wwwroot/";
          var dbPath = "assets/ImageDocument/" + fileName;
          var fullPath = pathToSave + dbPath;
          if (model.DocumentUrl != null)
          {
            Utils.MailUtils utils = new Utils.MailUtils();
            utils.DeleteImage(pathToSave + model.DocumentUrl);
          }

          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            file.CopyTo(stream);
          }
          model.DocumentUrl = dbPath;
        }

        _documentService.Update(model);
        return Ok();
      }
      catch (Exception ex)
      {
        return StatusCode(500);
      }

    }

    //[HttpGet]
    //[Route("active-document")]
    //public IActionResult Get()
    //{
    //  try
    //  {
    //    var currentDoc = _documentService.Get(x=> x.Status == DocStatus.Active,_=>_.Formula);

    //      return Ok(ModelMapper.ConvertToViewModel(currentDoc));
    //  }
    //  catch (Exception e)
    //  {
    //    return StatusCode(500, e);
    //  }
    //}

    [HttpGet]
    [Route("active-document")]
    [AllowAnonymous]
    public IActionResult Get(int m, int y)
    {
      try
      {
        var currentDoc = _documentService.Get(x => x.Status == DocStatus.Priority, _ => _.Formula.FormulaDetails);
        if (currentDoc == null)
        {
          currentDoc = _documentService.Get(x => x.Status == DocStatus.Active, _ => _.Formula.FormulaDetails);
        }

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

          if ((month == 4 || month == 6 || month == 9 || month == 11) && currentDoc.Deadline == 30)
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

        var formula = GetFormula(currentDoc.Formula);
        var output = new DocumentActiveVM
        {
          Id = currentDoc.Id,
          Code = currentDoc.Code,
          SignDate = currentDoc.SignDate,
          ApplyDate = currentDoc.ApplyDate,
          FromDate = fromdate,
          ToDate = todate,
          PayDate = paydate,
          Formula = formula.Formula
        };
        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    protected FormulaVM GetFormula(Formula formula)
    {
      FormulaVM result = new FormulaVM();

      result.Id = formula.Id;
      result.Name = formula.Name;
      result.Type = formula.IsSalaryFormula ? "CT Lương" : "CT Thành phần";
      result.CreateDate = formula.CreateDate;
      result.Description = formula.Description;
      result.FormulaDetails = new List<FormulaDetailVM>();
      string left = result.Name + " = ";
      string strFormular = left;
      formula.FormulaDetails.ToList().ForEach(fDetail =>
      {
        FormulaDetailVM detail = new FormulaDetailVM();
        detail.Id = fDetail.Id;
        detail.Operator = fDetail.Operator;
        detail.Type = fDetail.Type;

        switch (fDetail.Type)
        {
          case 1:

            var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
            var field = _fieldService.Get(_ => _.Id == fieldType.FieldId);
            detail.FieldTypeVM = new FieldTypeVM();
            detail.FieldTypeVM.Name = field.Name;
            break;
          case 2:
            var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.ReferenceTable.ReferenceTableDetails);

            var field2 = _fieldService.Get(_ => _.Id == refType.ReferenceTable.SourceValue);
            var refef = _referenceTableDetailService.Get(_ => _.Key == field2.Value);
            detail.RefTableTypeVM = new RefTableTypeVM();
            detail.RefTableTypeVM.Name = refType.ReferenceTable.Name;
            break;
          case 3:
            var formular = _formulaTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.Formula.FormulaDetails);
            detail.FormulaTypeVM = GetFormula(formular.Formula);
            break;
          case 4:
            var constantType = _constantTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
            detail.ConstantTypeVM = new ConstantTypeVM();
            detail.ConstantTypeVM.Value = constantType.Value; // ec
            break;
        }

        result.FormulaDetails.Add(detail);
      });

      // combine formula expression

      result.FormulaDetails.ToList().ForEach(_ =>
      {

        switch (_.Type)
        {
          case 1:
            strFormular += _.Operator.ToOperator() + _.FieldTypeVM.Name;
            break;
          case 2:
            strFormular += _.Operator.ToOperator() + _.RefTableTypeVM.Name;
            break;
          case 3:
            strFormular += _.Operator.ToOperator() + _.FormulaTypeVM.Name;
            break;
          case 4:
            strFormular += _.Operator.ToOperator() + _.ConstantTypeVM.Value;
            break;
        }
        strFormular += " ";

      });
      strFormular = strFormular.Replace(left + result.FormulaDetails.FirstOrDefault().Type.ToOperator(), left);
      strFormular = strFormular.Replace("= +", "=");
      result.Formula = strFormular;
      return result;
    }
  }

  public class PayrollReturnVM
  {
    public int month { get; set; }
    public int year { get; set; }
    public Document documentAfterCreate { get; set; }
  }
}
