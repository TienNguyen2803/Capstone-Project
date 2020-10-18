using BusinessLogic.Define;
using CapstoneUI.Utils;
using CapstoneUI.ViewModels;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CapstoneUI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class FormulaController : _BaseController
  {
    private readonly IFormulaService _formulaService;
    private readonly IPayslipTemplateService _payslipTemplateService;
    private readonly IPayslipService _payslipService;
    private readonly IPayrollService _payrollService;
    private readonly IFieldTypeService _fieldTypeService;
    private readonly IFormulaTypeService _formulaTypeService;
    private readonly IReferenceTableTypeService _referenceTableTypeService;
    private readonly IConstantTypeService _constantTypeService;
    private readonly IFieldService _fieldService;
    private readonly IReferenceTableDetailService _referenceTableDetailService;
    private readonly IReferenceTableService _referenceTableService;
    private readonly IFormulaDetailService _formulaDetailService;
    private readonly IDocumentService _documentService;
    public FormulaController(IFormulaService formulaService, IPayslipService payslipService, IPayrollService payrollService, IConstantTypeService constantTypeService,
      IFormulaTypeService formulaTypeService, IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService, IPayslipTemplateService payslipTemplateService,
      IFieldService fieldService, IDocumentService documentService,
      IReferenceTableDetailService referenceTableDetailService, IReferenceTableService referenceTableService, IFormulaDetailService formulaDetailService)
    {
      _payslipTemplateService = payslipTemplateService;
      _formulaService = formulaService;
      _payslipService = payslipService;
      _payrollService = payrollService;
      _constantTypeService = constantTypeService;
      _formulaTypeService = formulaTypeService;
      _fieldTypeService = fieldTypeService;
      _referenceTableTypeService = referenceTableTypeService;
      _fieldService = fieldService;
      _referenceTableDetailService = referenceTableDetailService;
      _referenceTableService = referenceTableService;
      _formulaDetailService = formulaDetailService;
      _documentService = documentService;
    }


    [HttpPost]
    //[Route("formula")]
    public IActionResult Create([FromBody] FormulaCreateVM viewModel)
    {
      try
      {
        _formulaService.Create(ModelMapper.ConvertToModel(viewModel),
          ModelMapper.ConvertToModel(viewModel.FormulaDetailCreateVMs), viewModel.DocId);

        return Ok(ModelMapper.ConvertToReturnViewModel(GetFormula(_formulaService.Get(_ => _.Name == viewModel.Name, _ => _.FormulaDetails))));
      }

      catch (Exception e)
      {
        return StatusCode(500, e.ToString());
      }

    }



    [HttpGet]
    [Route("formulas")]
    public IActionResult GetAllFor()
    {
      try
      {
        ICollection<FormulaVM> result = new List<FormulaVM>();
        var list = _formulaService.GetAll(_ => _.FormulaDetails).OrderByDescending(_ => _.CreateDate).ToList();
        list.ForEach(_ =>
       {
         result.Add(GetFormula(_));
       });
        return Ok(result);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }

    [HttpGet]
    [Route("fields")]
    public IActionResult GetAllFields()
    {
      try
      {
        ICollection<FieldVM> result = new List<FieldVM>();
        var list = _fieldService.GetAll().ToList();
        list.ForEach(_ =>
        {
          result.Add(ModelMapper.ConvertToViewModel(_));
        });
        return Ok(result);
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
      result.FormulaDetails.FirstOrDefault().Operator = 1;
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

    private delegate IActionResult CreateField(FieldCreateVM Fdetail);
    private delegate IActionResult CreateRef(ReferenceTableCreateVM Fdetail);
    private delegate string CreateDoc(DocumentCreateVM document);

    [HttpPost]
    [Route("orginal-file")]
    public IActionResult UploadFile(IFormFile file)
    {
      try
      {
        if (file != null)
        {
          var fileName = Path.GetFileName(file.FileName);
          var filePath = Path.Combine("assets/ImageDocument/", fileName);
          var uploads = Path.Combine(PathUtil.rootPath, filePath);
          file.CopyTo(new FileStream(uploads, FileMode.Create));
          return Ok(filePath);
        }
        return Ok(null);
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    [HttpPost]
    [Route("delete-orginal-file")]
    public IActionResult DeleteFileOrigin(string url)
    {
      try
      {
        DeleteFile(url);
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }
    }

    public class JsonModelBinder : IModelBinder
    {
      public Task BindModelAsync(ModelBindingContext bindingContext)
      {
        if (bindingContext == null)
        {
          throw new ArgumentNullException(nameof(bindingContext));
        }

        // Check the value sent in
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueProviderResult != ValueProviderResult.None)
        {
          bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

          // Attempt to convert the input value
          var valueAsString = valueProviderResult.FirstValue;
          var result = Newtonsoft.Json.JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType);
          if (result != null)
          {
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
          }
        }

        return Task.CompletedTask;
      }
    }

    //[HttpPost]
    //[Route("test")]
    //public IActionResult Test([ModelBinder(BinderType = typeof(JsonModelBinder))] DocumentCreateVM viewModel,
    //IFormFile file)
    //{
    //  try
    //  {
    //    if (file != null)
    //    {
    //      var fileName = Path.GetFileName(file.FileName);
    //      var filePath = Path.Combine("assets/ImageDocument/", fileName);
    //      var uploads = Path.Combine(PathUtil.rootPath, filePath);
    //      file.CopyTo(new FileStream(uploads, FileMode.Create));
    //    }
    //    return Ok(viewModel.ToString() + "\n Filename: " + file.FileName);
    //  }
    //  catch (Exception e)
    //  {
    //    return StatusCode(500, e);
    //  }
    //}

    private delegate IActionResult Scheduler();
    [HttpPost]
    [Route("formula")]
    public async Task<IActionResult> CreateDocument([ModelBinder(BinderType = typeof(JsonModelBinder))] DocumentCreateVM viewModel,
    IFormFile file)
    {
      try
      {
        if (file != null)
        {
          Random random = new Random();
          var filePath = Path.Combine("assets/ImageDocument/", viewModel.Code + "_Image.jpg");
          var uploads = Path.Combine(PathUtil.rootPath, filePath);
          using (var stream = new FileStream(uploads, FileMode.Create))
          {
            file.CopyTo(stream);
          }
          //file.CopyTo(new FileStream(uploads, FileMode.Create));
          viewModel.DocumentUrl = filePath;
        }
        else
        {
          viewModel.DocumentUrl = null;
        }
        //create doc
        var docVM = new DocumentCreateVM
        {
          ApplyDate = viewModel.ApplyDate,
          CloseDay = viewModel.CloseDay,
          Code = viewModel.Code,
          Deadline = viewModel.Deadline,
          Description = viewModel.Description,
          SignDate = viewModel.SignDate,
        };

        if (viewModel.DocumentUrl != null)
        {
          docVM.DocumentUrl = viewModel.DocumentUrl;
        }

        var docRs = (new CreateDoc(new DocumentController(_documentService).Create))(docVM);
        if (docRs != "ok")
        {
          DeleteFile(viewModel.DocumentUrl);
          return BadRequest(docRs);
        }

        var document = _documentService.Get(_ => _.Code == viewModel.Code);

        //create formula
        var formulaVM = viewModel.Formula;
        formulaVM.DocId = document.Id;
        var result = CreateFormula(formulaVM);



        #region check conflict document

        var currentDoc = _documentService.Get(_ => _.Status == DocStatus.Active);
        var day = document.ApplyDate.Value.Day;
        var month = document.ApplyDate.Value.Month;
        var year = document.ApplyDate.Value.Year;

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
            payroll.documentAfterCreate = document;

            var checkPayroll = _payrollService.Get(_ => _.Month == month && _.Year == year);
            if (checkPayroll != null)
            {
              if (checkPayroll.Status == PayrollStatus.New || checkPayroll.Status == PayrollStatus.Published)
              {
                return StatusCode(409, payroll);
              }
            }
            return StatusCode(409, payroll);
          }
        }
        #endregion
        #region active doc
        ///api/Schedule/check-payroll-sch
        var client = new HttpClient();
        var uri = new Uri("http://localhost:3911/api/Schedule/active-doc-sch");
        //var uri = new Uri("http://http://spcs.azurewebsites.net/Schedule/active-doc-sch");
        client.BaseAddress = uri;
        await client.GetAsync(uri);
        #endregion
        PayslipTemplate payslip = new PayslipTemplate();
        payslip.DocId = document.Id;
        payslip.TemplateUrl = @"\Resources\DocumentTemplate\Default.html";
        payslip.Status = true;
        _payslipTemplateService.Create(payslip);
        return Ok(ModelMapper.ConvertToViewModel(document));


      }
      catch (Exception e)
      {
        //if (viewModel.DocumentUrl != null)
        //{
        //  DeleteFile(viewModel.DocumentUrl);
        //}
        //DeleteFormula(viewModel.Formula);
        return StatusCode(500, e);
      }
    }

    private void DeleteFile(string url)
    {
      var filePath = Path.Combine(PathUtil.rootPath, url);
      if ((System.IO.File.Exists(filePath)))
      {
        var image = Image.FromFile(filePath);

        image.Dispose();
        System.IO.File.Delete(filePath);
      }
    }

    [HttpPost]
    [Route("test-create")]
    public IActionResult CreateeFormula([FromBody] FormulaNCVM viewModel)
    {
      try
      {
        //create doc 
        //CreateDoc createDoc = new CreateDoc(new DocumentController(_documentService).Create);
        //var docRs = createDoc();

        //assign docId to formula
        var docId = -1;
        if (viewModel.IsSalaryFormula)
        {
          //docId = _documentService.Get(_ => _.Id == docRs.Id).Id;
          docId = viewModel.DocId;
        }

        //converToModel
        var formulaVM = new FormulaCreateVM
        {
          Name = viewModel.Name,
          DocId = docId,
          Type = viewModel.FormulaType,
          Description = viewModel.Description,
          IsSalaryFormula = viewModel.IsSalaryFormula,
        };

        var details = new List<FormulaDetailCreateVM>();
        foreach (var fdetail in viewModel.FormulaDetailNCVMs)
        {
          var detail = new FormulaDetailCreateVM();
          detail.Operator = fdetail.Operator;
          detail.Type = fdetail.Type;
          var c = true;
          switch (fdetail.Type)
          {
            case 1:
              if (fdetail.FDTypeVM != null)
              {
                detail.FDType = new FormulaDetailTypeCreateVM { Id = fdetail.FDTypeVM.Id };
              }
              else
              {
                var fieldVM = new FieldCreateVM
                {
                  Name = fdetail.FieldTypeVM.Name,
                  DataType = fdetail.FieldTypeVM.DataType,
                  Description = fdetail.FieldTypeVM.Description,
                  IsMonthlySalaryComponent = fdetail.FieldTypeVM.IsMonthlySalaryComponent,
                  LongName = fdetail.FieldTypeVM.LongName
                };
                var existedfield = _fieldService.Get(_ => _.Name == fieldVM.Name);
                if (existedfield == null)
                {
                  CreateField createField = new CreateField(new FieldController(_fieldService).Create);
                  createField(fieldVM);
                }
                var field = _fieldService.Get(_ => _.Name == fieldVM.Name);
                detail.FDType = new FormulaDetailTypeCreateVM
                {
                  Id = field.Id
                };
              }
              break;
            case 2:
              if (fdetail.FDTypeVM != null)
              {
                detail.FDType = new FormulaDetailTypeCreateVM { Id = fdetail.FDTypeVM.Id };
              }
              else
              {
                var refVM = fdetail.RefTableTypeVM;
                var existedRef = _referenceTableService.Get(_ => _.Name == refVM.Name);
                if (existedRef == null)
                {
                  CreateRefTable(refVM);
                }
                var refTable = _referenceTableService.Get(_ => _.Name == refVM.Name, _ => _.ReferenceTableDetails);
                detail.FDType = new FormulaDetailTypeCreateVM
                {
                  Id = refTable.Id
                };
              }
              break;
            case 3:
              if (fdetail.FDTypeVM != null)
              {
                detail.FDType = new FormulaDetailTypeCreateVM { Id = fdetail.FDTypeVM.Id };
              }
              else
              {
                var formVM = fdetail.FormulaTypeVM;
                var existedForm = _formulaService.Get(_ => _.Name == formVM.Name);
                if (existedForm == null)
                {
                  CreateFormula(formVM);
                }
                var formula = _formulaService.Get(_ => _.Name == formVM.Name, _ => _.FormulaDetails, _ => _.FormulaTypes);
                detail.FDType = new FormulaDetailTypeCreateVM
                {
                  Id = formula.Id
                };
              }
              break;
            case 4:
              detail.FDType = new FormulaDetailTypeCreateVM
              {
                Value = fdetail.ConstantTypeVM.Value,
              };
              break;
          }
          details.Add(detail);
        }
        formulaVM.FormulaDetailCreateVMs = details;
        var result = Create(formulaVM);

        return result;

      }
      catch (Exception e)
      {
        //DeleteFormula(viewModel);
        return StatusCode(500, e.ToString());
      }

    }
    private IActionResult CreateFormula([FromBody] FormulaNCVM viewModel)
    {
      try
      {
        //create doc 
        //CreateDoc createDoc = new CreateDoc(new DocumentController(_documentService).Create);
        //var docRs = createDoc();

        //assign docId to formula
        var docId = -1;
        if (viewModel.IsSalaryFormula)
        {
          //docId = _documentService.Get(_ => _.Id == docRs.Id).Id;
          docId = viewModel.DocId;
        }

        //converToModel
        var formulaVM = new FormulaCreateVM
        {
          Name = viewModel.Name,
          DocId = docId,
          Type = viewModel.FormulaType,
          Description = viewModel.Description,
          IsSalaryFormula = viewModel.IsSalaryFormula,
        };

        var details = new List<FormulaDetailCreateVM>();
        foreach (var fdetail in viewModel.FormulaDetailNCVMs)
        {
          var detail = new FormulaDetailCreateVM();
          detail.Operator = fdetail.Operator;
          detail.Type = fdetail.Type;
          var c = true;
          switch (fdetail.Type)
          {
            case 1:
              if (fdetail.FDTypeVM != null)
              {
                detail.FDType = new FormulaDetailTypeCreateVM { Id = fdetail.FDTypeVM.Id };
              }
              else
              {
                var fieldVM = new FieldCreateVM
                {
                  Name = fdetail.FieldTypeVM.Name,
                  DataType = fdetail.FieldTypeVM.DataType,
                  Description = fdetail.FieldTypeVM.Description,
                  IsMonthlySalaryComponent = fdetail.FieldTypeVM.IsMonthlySalaryComponent,
                  LongName = fdetail.FieldTypeVM.LongName
                };
                var existedfield = _fieldService.Get(_ => _.Name == fieldVM.Name);
                if (existedfield == null)
                {
                  CreateField createField = new CreateField(new FieldController(_fieldService).Create);
                  createField(fieldVM);
                }
                var field = _fieldService.Get(_ => _.Name == fieldVM.Name);
                detail.FDType = new FormulaDetailTypeCreateVM
                {
                  Id = field.Id
                };
              }
              break;
            case 2:
              if (fdetail.FDTypeVM != null)
              {
                detail.FDType = new FormulaDetailTypeCreateVM { Id = fdetail.FDTypeVM.Id };
              }
              else
              {
                var refVM = fdetail.RefTableTypeVM;
                var existedRef = _referenceTableService.Get(_ => _.Name == refVM.Name);
                if (existedRef == null)
                {
                  CreateRefTable(refVM);
                  //CreateRefTableOD(refVM);
                }
                var refTable = _referenceTableService.Get(_ => _.Name == refVM.Name, _ => _.ReferenceTableDetails);
                detail.FDType = new FormulaDetailTypeCreateVM
                {
                  Id = refTable.Id
                };
              }
              break;
            case 3:
              if (fdetail.FDTypeVM != null)
              {
                detail.FDType = new FormulaDetailTypeCreateVM { Id = fdetail.FDTypeVM.Id };
              }
              else
              {
                var formVM = fdetail.FormulaTypeVM;
                var existedForm = _formulaService.Get(_ => _.Name == formVM.Name);
                if (existedForm == null)
                {
                  CreateFormula(formVM);
                }
                var formula = _formulaService.Get(_ => _.Name == formVM.Name, _ => _.FormulaDetails, _ => _.FormulaTypes);
                detail.FDType = new FormulaDetailTypeCreateVM
                {
                  Id = formula.Id
                };
              }
              break;
            case 4:
              detail.FDType = new FormulaDetailTypeCreateVM
              {
                Value = fdetail.ConstantTypeVM.Value,
              };
              break;
          }
          details.Add(detail);
        }
        details.FirstOrDefault().Operator = 1;
        formulaVM.FormulaDetailCreateVMs = details;
        var result = Create(formulaVM);

        var returnVM = ModelMapper.ConvertToReturnViewModel(GetFormula(_formulaService.Get(_ => _.Name == viewModel.Name, _ => _.FormulaDetails)));
        return result;

      }
      catch (Exception e)
      {
        //DeleteFormula(viewModel);
        return StatusCode(500, e.ToString());
      }

    }

    [HttpPost]
    [Route("referencetables-testcreate")]
    public IActionResult CreateREF([FromBody] RefTableNCVM viewModel)
    {
      try
      {
        CreateRefTable(viewModel);
        return Ok();
      }
      catch (Exception e)
      {
        return StatusCode(500, e);
      }

    }
    private void CreateRefTable(RefTableNCVM viewModel)
    {
      int sValueId;
      var refVM = new ReferenceTableCreateVM
      {
        Name = viewModel.Name,
        Description = viewModel.Description,
        ReturnType = viewModel.ReturnType,
        SourceType = viewModel.SourceType,
        CompareType = viewModel.CompareType,
      };
      var c = true;
      switch (viewModel.SourceType)
      {

        case 1:
          if (_fieldService.Get(_ => _.Name == viewModel.SourceName) == null)
          {
            var fieldVM1 = new FieldCreateVM
            {
              Name = viewModel.FieldVM.Name,
              DataType = viewModel.FieldVM.DataType,
              Description = viewModel.FieldVM.Description,
              IsMonthlySalaryComponent = viewModel.FieldVM.IsMonthlySalaryComponent,
              LongName = viewModel.FieldVM.LongName
            };
            var existedfield = _fieldService.Get(_ => _.Name == fieldVM1.Name);
            if (existedfield == null)
            {
              CreateField createField = new CreateField(new FieldController(_fieldService).Create);
              createField(fieldVM1);
            }
          }
          sValueId = _fieldService.Get(_ => _.Name == viewModel.SourceName).Id;
          refVM.SourceValue = sValueId;
          break;
        case 2:
          if (_referenceTableService.Get(_ => _.Name == viewModel.SourceName, _ => _.ReferenceTableDetails) == null)
          {
            var refVM1 = viewModel.ReferenceVM;
            var existedRef = _referenceTableService.Get(_ => _.Name == refVM.Name);
            if (existedRef == null)
            {
              CreateRefTable(refVM1);
            }
          }
          sValueId = _referenceTableService.Get(_ => _.Name == viewModel.SourceName, _ => _.ReferenceTableDetails).Id;
          refVM.SourceValue = sValueId;
          break;
        case 3:
          if (_formulaService.Get(_ => _.Name == viewModel.SourceName, _ => _.FormulaDetails) == null)
          {
            var form1 = viewModel.FormulaTypeVM;
            var existedForm = _formulaService.Get(_ => _.Name == form1.Name);
            if (existedForm == null)
            {
              CreateFormula(form1);
            }
          }
          sValueId = _formulaService.Get(_ => _.Name == viewModel.SourceName, _ => _.FormulaDetails).Id;
          refVM.SourceValue = sValueId;
          break;
      }

      foreach (var fd in viewModel.ReferenceTableDetailCreateVMs)
      {
        switch (viewModel.ReturnType)
        {
          case "1":
            if (_fieldService.Get(_ => _.Name == fd.Value) == null)
            {
              var fieldVM1 = new FieldCreateVM
              {
                Name = fd.FieldVM.Name,
                DataType = fd.FieldVM.DataType,
                Description = fd.FieldVM.Description,
                IsMonthlySalaryComponent = fd.FieldVM.IsMonthlySalaryComponent,
                LongName = fd.FieldVM.LongName
              };
              var existedfield = _fieldService.Get(_ => _.Name == fieldVM1.Name);
              if (existedfield == null)
              {
                CreateField createField = new CreateField(new FieldController(_fieldService).Create);
                createField(fieldVM1);
              }
            }
            break;
          case "2":
            if (_referenceTableService.Get(_ => _.Name == fd.Value, _ => _.ReferenceTableDetails) == null)
            {
              var refVM1 = fd.ReferenceVM;
              var existedRef = _referenceTableService.Get(_ => _.Name == refVM.Name);
              if (existedRef == null)
              {
                CreateRefTable(refVM1);
              }
            }
            break;
          case "3":
            if (_formulaService.Get(_ => _.Name == fd.Value, _ => _.FormulaDetails) == null)
            {
              var form1 = fd.FormulaTypeVM;
              var existedForm = _formulaService.Get(_ => _.Name == form1.Name);
              if (existedForm == null)
              {
                CreateFormula(form1);
              }
            }
            break;
        }
      }
      refVM.ReferenceTableDetailCreateVMs = viewModel.ReferenceTableDetailCreateVMs;

      CreateRef createRef = new CreateRef(new ReferenceTableController(_referenceTableService).Create);
      createRef(refVM);
    }
    private void DeleteFormula(FormulaNCVM viewModel)
    {
      _documentService.Delete(_documentService.Get(_ => _.Id == viewModel.DocId));
      var formula = _formulaService.Get(_ => _.Name == viewModel.Name, _ => _.FormulaDetails, _ => _.FormulaTypes);
      if (formula != null)
      {
        _formulaDetailService.GetAll(_ => _.ConstantType, _ => _.FieldType, _ => _.FormulaType, _ => _.ReferenceTableType)
          .Where(_ => _.FormulaId == formula.Id).ToList().ForEach(fd =>
          {
            switch (fd.Type)
            {
              case 1:
                _fieldTypeService.Delete(fd.FieldType);
                break;
              case 2:
                _referenceTableTypeService.Delete(fd.ReferenceTableType);
                break;
              case 3:
                _formulaTypeService.Delete(fd.FormulaType);
                break;
              case 4:
                _constantTypeService.Delete(fd.ConstantType);
                break;
            }
          });
        _formulaDetailService.Delete(formula.FormulaDetails);
        _formulaTypeService.Delete(formula.FormulaTypes);
        _formulaService.Delete(formula);
      }
      else
      {
        foreach (var fdetail in viewModel.FormulaDetailNCVMs)
        {
          var detail = new FormulaDetailCreateVM();
          detail.Type = fdetail.Type;
          switch (fdetail.Type)
          {
            case 1:
              if (fdetail.FDTypeVM == null)
              {
                var field = _fieldService.Get(_ => _.Name == fdetail.FieldTypeVM.Name);
                if (field != null)
                {
                  _fieldService.Delete(field);
                }
              }
              break;
            case 2:
              if (fdetail.FDTypeVM == null)
              {
                var refTable = _referenceTableService.Get(_ => _.Name == fdetail.RefTableTypeVM.Name);
                if (refTable != null)
                {
                  if (refTable.ReferenceTableDetails.Count != 0)
                  {
                    _referenceTableDetailService.Delete(refTable.ReferenceTableDetails);
                  }
                  _referenceTableService.Delete(refTable);
                }
              }
              break;
            case 3:
              if (fdetail.FDTypeVM == null)
              {
                var formVM = fdetail.FormulaTypeVM;
                DeleteFormula(formVM);
              }
              break;
            case 4:
              break;
          }
        }
      }
    }

    private ICollection<string> errorField = new HashSet<string>();

    [HttpPost]
    [Route("check-formula-field")]
    public IActionResult CheckFields([FromBody] FormulaNCVM viewModel)
    {
      try
      {
        ICollection<FieldCVM> fields = new HashSet<FieldCVM>();
        var output = CFFormula(viewModel, fields).ToHashSet();
        var err = errorField.ToHashSet();
        if (err.Count != 0)
        {
          var message = "";
          foreach (var item in err)
          {
            message = message + item + ", ";
          }
          message = message.Remove(message.Trim().Length - 1).Trim();
          return BadRequest(message);
        }
        return Ok(output);
      }
      catch (Exception e)
      {
        return StatusCode(500, e.ToString());
      }
    }

    private ICollection<FieldCVM> CFFormula(FormulaNCVM viewModel, ICollection<FieldCVM> fields)
    {
      foreach (var fdetail in viewModel.FormulaDetailNCVMs)
      {
        switch (fdetail.Type)
        {
          case 1:
            if (fdetail.FDTypeVM != null)
            {
              fields.Add(new FieldCVM { Name = _fieldService.Get(_ => _.Id == fdetail.FDTypeVM.Id).Name });
            }
            else
            {
              fields.Add(new FieldCVM { Name = fdetail.FieldTypeVM.Name });
            }
            break;
          case 2:
            if (fdetail.FDTypeVM != null)
            {
              fields = fields.Union(CFERef(_referenceTableService.Get(_ => _.Id == fdetail.FDTypeVM.Id, _ => _.ReferenceTableDetails))).ToHashSet();
            }
            else
              fields = fields.Union(CFRef(fdetail.RefTableTypeVM)).ToHashSet();
            break;
          case 3:
            if (fdetail.FDTypeVM != null)
            {
              var ffield = ModelMapper.ConvertToFCViewModel(GetFieldByFormula(_formulaService.Get(_ => _.Id == fdetail.FDTypeVM.Id, _ => _.FormulaDetails)));

              fields = fields.Union(ffield).ToHashSet();
            }
            else
              fields = fields.Union(CFFormula(fdetail.FormulaTypeVM, fields)).ToHashSet();
            break;
          case 4:
            break;
          default:
            errorField.Add(fdetail.Name);
            break;
        }
      }
      return fields.ToHashSet();
    }

    private ICollection<FieldCVM> CFRef(RefTableNCVM viewModel)
    {
      ICollection<FieldCVM> fields = new HashSet<FieldCVM>();

      switch (viewModel.SourceType)
      {
        default:
          fields.Add(new FieldCVM { Name = viewModel.SourceName });
          break;
        case 2:
          if (_referenceTableService.Get(_ => _.Name == viewModel.SourceName, _ => _.ReferenceTableDetails) != null)
            fields = fields.Union(CFERef(_referenceTableService.Get(_ => _.Name == viewModel.SourceName, _ => _.ReferenceTableDetails))).ToHashSet();
          else
            fields = fields.Union(CFRef(viewModel.ReferenceVM)).ToHashSet();
          break;
        case 3:
          if (_formulaService.Get(_ => _.Name == viewModel.SourceName) != null)
            fields = fields.Union(ModelMapper.ConvertToFCViewModel(GetFieldByFormula(_formulaService.Get(_ => _.Name == viewModel.SourceName, _ => _.FormulaDetails)))).ToHashSet();
          else
            fields = fields.Union(CFFormula(viewModel.FormulaTypeVM, fields)).ToHashSet();
          break;
        case -1:
          errorField.Add(viewModel.SourceName);
          break;
      }

      foreach (var fd in viewModel.ReferenceTableDetailCreateVMs)
      {
        switch (viewModel.ReturnType)
        {
          case "1":
            fields.Add(new FieldCVM { Name = fd.Value });
            break;
          case "2":
            if (_referenceTableService.Get(_ => _.Name == fd.Value, _ => _.ReferenceTableDetails) != null)
              fields = fields.Union(CFERef(_referenceTableService.Get(_ => _.Name == fd.Value, _ => _.ReferenceTableDetails))).ToHashSet();
            else
              fields = fields.Union(CFRef(fd.ReferenceVM)).ToHashSet();
            break;
          case "3":
            if (_formulaService.Get(_ => _.Name == fd.Value, _ => _.FormulaDetails) != null)
              fields = fields.Union(ModelMapper.ConvertToFCViewModel(GetFieldByFormula(_formulaService.Get(_ => _.Name == fd.Value, _ => _.FormulaDetails)))).ToHashSet();
            else
              fields = fields.Union(CFFormula(fd.FormulaTypeVM, fields)).ToHashSet();
            break;
          default:
            break;
        }
      }

      return fields.ToHashSet();
    }

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
      return list.ToHashSet();
    }
    private ICollection<FieldCVM> CFERef(ReferenceTable model)
    {
      ICollection<FieldCVM> result = new HashSet<FieldCVM>();
      switch (model.SourceType)
      {
        default:
          result.Add(new FieldCVM { Name = _fieldService.Get(_ => _.Id == model.SourceValue).Name });
          break;
        case 2:
          result = result.Union(CFERef(_referenceTableService.Get(_ => _.Id == model.SourceValue, _ => _.ReferenceTableDetails))).ToHashSet();
          break;
        case 3:
          result = result.Union(ModelMapper.ConvertToFCViewModel(GetFieldByFormula(_formulaService.Get(_ => _.Id == model.SourceValue, _ => _.FormulaDetails)))).ToHashSet();
          break;
      }

      foreach (var fd in model.ReferenceTableDetails)
      {
        switch (model.ReturnType)
        {
          case "1":
            result.Add(new FieldCVM { Name = _fieldService.Get(_ => _.Id.ToString() == fd.Value).Name });
            break;
          case "2":
            result = result.Union(CFERef(_referenceTableService.Get(_ => _.Id.ToString() == fd.Value, _ => _.ReferenceTableDetails))).ToHashSet();
            break;
          case "3":
            result = result.Union(ModelMapper.ConvertToFCViewModel(GetFieldByFormula(_formulaService.Get(_ => _.Id.ToString() == fd.Value, _ => _.FormulaDetails)))).ToHashSet();
            break;
          default:
            break;
        }
      }
      return result.ToHashSet();
    }

    public static ICollection<FormulaElementNCVM> output;
    public static ICollection<FormulaShowVM> formulaSVM;

    [HttpPost]
    [Route("check-formula")]
    public IActionResult CheckFormula([FromBody] FormulaNCVM viewModel)
    {
      try
      {
        ICollection<Object> outputs = new List<Object>();
        ICollection<Object> formulaSVMs = new List<Object>();
        foreach (var fieldsAndValues in viewModel.FieldsandValuesTest)
        {
          ICollection<FormulaNCVM> formulas = new HashSet<FormulaNCVM>();
          ICollection<RefTypeSSVM> fields = new HashSet<RefTypeSSVM>();
          ICollection<RefTableNCVM> reftbs = new HashSet<RefTableNCVM>();
          ICollection<FieldCVM> fieldsValues = fieldsAndValues;
          output = new HashSet<FormulaElementNCVM>();
          formulaSVM = new HashSet<FormulaShowVM>();

          var formula = cFormula(viewModel, output, formulaSVM, fieldsValues, formulas, fields, reftbs);
          outputs.Add(output);
        }
        return Ok(outputs);
        //return Ok(formula);
      }
      catch (Exception e)
      {
        return StatusCode(500, e.ToString());
      }
    }

    [HttpPost]
    [Route("show-formula")]
    public IActionResult FormulaShowVM([FromBody] FormulaNCVM viewModel)
    {
      try
      {
        ICollection<Object> outputs = new List<Object>();
        ICollection<Object> formulaSVMs = new List<Object>();
        foreach (var fieldsAndValues in viewModel.FieldsandValuesTest)
        {
          ICollection<FormulaNCVM> formulas = new HashSet<FormulaNCVM>();
          ICollection<RefTypeSSVM> fields = new HashSet<RefTypeSSVM>();
          ICollection<RefTableNCVM> reftbs = new HashSet<RefTableNCVM>();
          ICollection<FieldCVM> fieldsValues = fieldsAndValues;
          output = new HashSet<FormulaElementNCVM>();
          formulaSVM = new HashSet<FormulaShowVM>();

          var formula = cFormula(viewModel, output, formulaSVM, fieldsValues, formulas, fields, reftbs);
          outputs.Add(output);
          formulaSVMs.Add(formulaSVM);
        }
        return Ok(formulaSVMs);
        //return Ok(formula);
      }
      catch (Exception e)
      {
        return StatusCode(500, e.ToString());
      }
    }
    private FormulaTypeSSVM cFormula(FormulaNCVM viewModel, ICollection<FormulaElementNCVM> output, ICollection<FormulaShowVM> formulaSVM, ICollection<FieldCVM> fieldsValues, ICollection<FormulaNCVM> formulas, ICollection<RefTypeSSVM> fields, ICollection<RefTableNCVM> reftbs)
    {
      var result = new FormulaTypeSSVM
      {
        Name = viewModel.Name,
        FormulaType = viewModel.FormulaType,
      };
      var details = new List<FormulaDetailSSVM>();
      foreach (var fdetail in viewModel.FormulaDetailNCVMs)
      {
        var detail = new FormulaDetailSSVM();
        detail.Operator = fdetail.Operator;
        detail.Type = fdetail.Type;
        switch (fdetail.Type)
        {
          case 1:
            var fl = new FieldCVM();
            if (fdetail.FDTypeVM != null)
            {
              fl = fieldsValues.FirstOrDefault(_ => _.Name == _fieldService.Get(__ => __.Id == fdetail.FDTypeVM.Id).Name);
            }
            else
            {
              fl = fieldsValues.FirstOrDefault(_ => _.Name == fdetail.FieldTypeVM.Name);
            }
            detail.FieldType = new RefTypeSSVM
            {
              Name = fl.Name,
              Value = fl.Value
            };
            output.Add(new FormulaElementNCVM
            {
              Name = fl.Name,
              Value = fl.Value,
              Type = 1
            });
            fields.Add(detail.FieldType);
            break;
          case 2:
            var retb = new RefTypeSSVM();
            if (fdetail.FDTypeVM != null)
            {
              retb = cERefTb(_referenceTableService.Get(_ => _.Id == fdetail.FDTypeVM.Id, _ => _.ReferenceTableDetails), output, fieldsValues);
            }
            else
              retb = cRefTb(fdetail.RefTableTypeVM, fieldsValues, formulas, fields, reftbs);
            detail.RefType = new RefTypeSSVM
            {
              Name = retb.Name,
              Key = retb.Key,
              Value = retb.Value
            };
            output.Add(new FormulaElementNCVM
            {
              Name = retb.Name,
              Value = retb.Value,
              Type = 2
            });
            reftbs.Add(fdetail.RefTableTypeVM);
            break;
          case 3:
            var frm = new FormulaTypeSSVM();
            if (fdetail.FDTypeVM != null)
            {
              frm = cEFormula(_formulaService.Get(_ => _.Id == fdetail.FDTypeVM.Id, _ => _.FormulaDetails), output, fieldsValues);
            }
            else
              frm = cFormula(fdetail.FormulaTypeVM, output, formulaSVM, fieldsValues, formulas, fields, reftbs);

            detail.FormulaType = new FormulaTypeSSVM
            {
              Name = frm.Name,
              Value = frm.Value,
              FormulaType = frm.FormulaType,
              Formula = frm.Formula,
              Details = frm.Details
            };
            output.Add(new FormulaElementNCVM
            {
              Name = frm.Name,
              Value = frm.Value.ToString(),
              Expression = frm.Formula,
              Type = 3
            });
            formulaSVM.Add(new FormulaShowVM
            {
              Name = frm.Name,
              Expression = frm.Formula
            });
            formulas.Add(fdetail.FormulaTypeVM);
            break;
          case 4:
            detail.ConstantType = new ConstantTypeSSVM { Value = fdetail.ConstantTypeVM.Value };
            break;
        }
        details.Add(detail);
      }
      result.Details = details;
      result.Details.FirstOrDefault().Operator = 1;

      #region evaluate

      string left = result.Name + " = ";
      string strFormular = left;

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
      switch (viewModel.FormulaType)
      {
        case 1:
          try
          {
            result.Value = CalculateUtil.evaluate(expression);
          }
          catch (Exception)
          {
            result.Value = 0;
          }
          result.Formula = strFormular;
          break;
        case 2:
          try
          {
            result.Value = result.Details.Min(_ => Decimal.Parse(_.Value));
          }
          catch (Exception)
          {
            result.Value = 0;
          }
          result.Formula = result.Name;
          break;
        case 3:
          try
          {
            result.Value = result.Details.Max(_ => Decimal.Parse(_.Value));
          }
          catch (Exception)
          {
            result.Value = 0;
          }
          result.Formula = result.Name;
          break;
        case 4:
          try
          {
            result.Value = result.Details.Average(_ => Decimal.Parse(_.Value));
          }
          catch (Exception)
          {
            result.Value = 0;
          }
          result.Formula = result.Name;
          break;
        case 5:
          try
          {
            result.Value = result.Details.Max(_ => Decimal.Parse(_.Value)) - result.Details.Min(_ => Decimal.Parse(_.Value));
          }
          catch (Exception)
          {
            result.Value = 0;
          }
          result.Formula = result.Name;
          break;
      }
      output.Add(new FormulaElementNCVM
      {
        Name = result.Name,
        Value = result.Value.ToString(),
        Expression = result.Formula,
        Type = 3
      });
      formulaSVM.Add(new FormulaShowVM
      {
        Name = result.Name,
        Expression = result.Formula
      });
      return result;
      #endregion
    }

    private RefTypeSSVM cRefTb(RefTableNCVM viewModel, ICollection<FieldCVM> fieldsValues, ICollection<FormulaNCVM> formulas, ICollection<RefTypeSSVM> fields, ICollection<RefTableNCVM> reftbs)
    {
      var rtb = new RefTypeSSVM();
      var rdetal = new ReferenceTableDetailCreateVM();
      if (viewModel.CompareType == 1)
      {
        switch (viewModel.SourceType)
        {
          default:
            var fi = fieldsValues.FirstOrDefault(_ => _.Name == viewModel.SourceName);
            rdetal = viewModel.ReferenceTableDetailCreateVMs.
              FirstOrDefault(_ => _.Key == fi.Value);
            break;
          case 2:
            var rf = reftbs.FirstOrDefault(_ => _.Name == viewModel.SourceName);
            var rtb1 = cRefTb(rf, fieldsValues, formulas, fields, reftbs);
            rdetal = viewModel.ReferenceTableDetailCreateVMs.
                FirstOrDefault(_ => _.Key == rtb1.Value);
            break;
          case 3:
            var form = formulas.FirstOrDefault(_ => _.Name == viewModel.SourceName);
            var formu = cFormula(form, output, formulaSVM, fieldsValues, formulas, fields, reftbs);
            rdetal = viewModel.ReferenceTableDetailCreateVMs.FirstOrDefault(_ => _.Key == formu.Value.ToString());
            break;
        }
      }
      else
      {
        var rdetail = viewModel.ReferenceTableDetailCreateVMs.Select(rd => new ReferenceTableDetailCreateVM
        {
          Key = rd.Key,
          Value = rd.Value,
          Start = Decimal.Parse(rd.Key.Split(",")[0].Trim()),
          End = Decimal.Parse(rd.Key.Split(",")[1].Trim())
        });
        switch (viewModel.SourceType)
        {
          default:
            if (fields.Count != 0)
            {
              var fi = fieldsValues.FirstOrDefault(_ => _.Name == viewModel.SourceName);
              rdetal = rdetail.
                FirstOrDefault(_ => _.Start <= Decimal.Parse(fi.Value) && _.End >= Decimal.Parse(fi.Value));
            }
            break;
          case 2:
            var rf = reftbs.FirstOrDefault(_ => _.Name == viewModel.SourceName);
            var rtb1 = cRefTb(rf, fieldsValues, formulas, fields, reftbs);
            rdetal = rdetail.
                FirstOrDefault(_ => _.Start <= Decimal.Parse(rtb1.Value) && _.End >= Decimal.Parse(rtb1.Value));
            break;
          case 3:
            var form = formulas.FirstOrDefault(_ => _.Name == viewModel.SourceName);
            var formu = cFormula(form, output, formulaSVM, fieldsValues, formulas, fields, reftbs);
            rdetal = rdetail.FirstOrDefault(_ => _.Start <= formu.Value && _.End >= formu.Value);
            break;
        }
      }

      rtb.Name = viewModel.Name;
      if (rdetal != null)
      {
        switch (viewModel.ReturnType)
        {
          case "1":
            if (fields.FirstOrDefault(_ => _.Name == rdetal.Value) != null)
            {
              rtb.Value = fields.FirstOrDefault(_ => _.Name == rdetal.Value).Value;
            }
            else
            {
              rtb.Value = fieldsValues.FirstOrDefault(_ => _.Name == rdetal.Value).Value;
              fields.Add(new RefTypeSSVM
              {
                Name = fieldsValues.FirstOrDefault(_ => _.Name == rdetal.Value).Name,
                Value = fieldsValues.FirstOrDefault(_ => _.Name == rdetal.Value).Value
              });
            }
            break;
          case "2":
            if (reftbs.FirstOrDefault(_ => _.Name == rdetal.Value) != null)
            {
              rtb.Value = cRefTb(reftbs.FirstOrDefault(_ => _.Name == rdetal.Value), fieldsValues, formulas, fields, reftbs).Value;
            }
            else
              rtb.Value = cRefTb(rdetal.ReferenceVM, fieldsValues, formulas, fields, reftbs).Value;
            break;
          case "3":
            if (formulas.FirstOrDefault(_ => _.Name == rdetal.Value) != null)
            {
              rtb.Value = cFormula(formulas.FirstOrDefault(_ => _.Name == rdetal.Value), output, formulaSVM, fieldsValues, formulas, fields, reftbs).Value.ToString();
            }
            else
              rtb.Value = cFormula(rdetal.FormulaTypeVM, output, formulaSVM, fieldsValues, formulas, fields, reftbs).Value.ToString();
            break;
          default:
            rtb.Value = rdetal.Value;
            break;
        }
        rtb.Key = viewModel.SourceName + ": " + rdetal.Key;
      }
      else
      {
        rtb.Value = "0";
        rtb.Key = null;
      }

      return rtb;
    }

    private FormulaTypeSSVM cEFormula(Formula formula, ICollection<FormulaElementNCVM> output, ICollection<FieldCVM> fieldsValues)
    {
      try
      {
        FormulaTypeSSVM result = new FormulaTypeSSVM();

        result.Name = formula.Name;
        result.Details = new List<FormulaDetailSSVM>();
        formula.FormulaDetails.ToList().ForEach(fDetail =>
        {
          FormulaDetailSSVM detail = new FormulaDetailSSVM();
          detail.Id = fDetail.Id;
          detail.Operator = fDetail.Operator;
          detail.Type = fDetail.Type;

          switch (fDetail.Type)
          {
            case 1:

              var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
              var field = fieldsValues.FirstOrDefault(_ => _.Name == _fieldService.Get(__ => __.Id == fieldType.FieldId).Name);
              detail.FieldType = new RefTypeSSVM();
              detail.FieldType.Name = field.Name;
              detail.FieldType.Value = field.Value;
              output.Add(new FormulaElementNCVM
              {
                Name = field.Name,
                Value = field.Value,
                Type = 1
              });
              break;
            case 2:
              var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.ReferenceTable.ReferenceTableDetails);
              detail.RefType = cERefTb(_referenceTableService.Get(_ => _.Id == refType.RefenceTableTypeId, _ => _.ReferenceTableDetails), output, fieldsValues);
              break;
            case 3:
              var formular = _formulaTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.Formula.FormulaDetails);
              detail.FormulaType = cEFormula(_formulaService.Get(_ => _.Id == formular.FormulaId, _ => _.FormulaDetails), output, fieldsValues);
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
        string left = result.Name + " = ";
        string strFormular = left;
        string expression = "0";
        result.Details.FirstOrDefault().Operator = 1;
        result.Details.ToList().ForEach(_ =>
        {
          expression += " ";

          switch (_.Type)
          {
            case 1:
              expression += _.Operator.ToOperator();
              expression += " " + fieldsValues.FirstOrDefault(__ => __.Name == _.FieldType.Name).Value;
              strFormular += _.Operator.ToOperator() + _.FieldType.Name;
              _.Value = fieldsValues.FirstOrDefault(__ => __.Name == _.FieldType.Name).Value;
              break;
            case 2:
              expression += _.Operator.ToOperator();
              expression += " " + cERefTb(_referenceTableService.Get(__ => __.Name == _.RefType.Name), output, fieldsValues).Value;
              strFormular += _.Operator.ToOperator() + _.RefType.Name;
              _.Value = cERefTb(_referenceTableService.Get(__ => __.Name == _.RefType.Name), output, fieldsValues).Value;
              break;
            case 3:
              expression += _.Operator.ToOperator();
              expression += " " + cEFormula(_formulaService.Get(__ => __.Name == _.FormulaType.Name), output, fieldsValues).Value;
              strFormular += _.Operator.ToOperator() + _.FormulaType.Name;
              _.Value = cEFormula(_formulaService.Get(__ => __.Name == _.FormulaType.Name), output, fieldsValues).Value.ToString();
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
            try
            {
              result.Value = CalculateUtil.evaluate(expression);
            }
            catch (Exception)
            {
              result.Value = 0;
            }
            result.Formula = strFormular;
            break;
          case 2:
            try
            {
              result.Value = result.Details.Min(_ => Decimal.Parse(_.Value));
            }
            catch (Exception)
            {
              result.Value = 0;
            }
            result.Formula = result.Name;
            break;
          case 3:
            try
            {
              result.Value = result.Details.Max(_ => Decimal.Parse(_.Value));
            }
            catch (Exception)
            {
              result.Value = 0;
            }
            result.Formula = result.Name;
            break;
          case 4:
            try
            {
              result.Value = result.Details.Average(_ => Decimal.Parse(_.Value));
            }
            catch (Exception)
            {
              result.Value = 0;
            }
            result.Formula = result.Name;
            break;
          case 5:
            try
            {
              result.Value = result.Details.Max(_ => Decimal.Parse(_.Value)) - result.Details.Min(_ => Decimal.Parse(_.Value));
            }
            catch (Exception)
            {
              result.Value = 0;
            }
            result.Formula = result.Name;
            break;
        }

        return result;
      }
      catch (Exception e)
      {
        throw;
      }
    }
    private RefTypeSSVM cERefTb(ReferenceTable model, ICollection<FormulaElementNCVM> output, ICollection<FieldCVM> fieldsValues)
    {
      var rtb = new RefTypeSSVM();
      var rdetal = new ReferenceTableDetailCreateVM();
      try
      {
        if (model.CompareType == 1)
        {
          switch (model.SourceType)
          {
            default:
              var fi = fieldsValues.FirstOrDefault(_ => _.Name == _fieldService.Get(__ => __.Id == model.SourceValue).Name);
              rdetal = new ReferenceTableDetailCreateVM
              {
                Key = model.ReferenceTableDetails.FirstOrDefault(_ => _.Key == fi.Value).Key,
                Value = model.ReferenceTableDetails.FirstOrDefault(_ => _.Key == fi.Value).Value
              };
              break;
            case 2:
              var rf = _referenceTableService.Get(_ => _.Id == model.SourceValue, _ => _.ReferenceTableDetails);
              var rtb1 = cERefTb(rf, output, fieldsValues);
              rdetal = new ReferenceTableDetailCreateVM
              {
                Key = model.ReferenceTableDetails.FirstOrDefault(_ => _.Key == rtb1.Value).Key,
                Value = model.ReferenceTableDetails.FirstOrDefault(_ => _.Key == rtb1.Value).Value
              };
              break;
            case 3:
              var form = _formulaService.Get(_ => _.Id == model.SourceValue, _ => _.FormulaDetails);
              //var formu = cFormula(form, fieldsValues, formulas, fields, reftbs);
              var formu = cEFormula(form, output, fieldsValues);
              rdetal = new ReferenceTableDetailCreateVM
              {
                Key = model.ReferenceTableDetails.FirstOrDefault(_ => _.Key == formu.Value.ToString()).Key,
                Value = model.ReferenceTableDetails.FirstOrDefault(_ => _.Key == formu.Value.ToString()).Value
              };
              break;
          }
        }
        else
        {
          var rdetail = model.ReferenceTableDetails.Select(rd => new ReferenceTableDetailCreateVM
          {
            Key = rd.Key,
            Value = rd.Value,
            Start = Decimal.Parse(rd.Key.Split(",")[0].Trim()),
            End = Decimal.Parse(rd.Key.Split(",")[1].Trim())
          });
          switch (model.SourceType)
          {
            default:
              var fi = fieldsValues.FirstOrDefault(_ => _.Name == _fieldService.Get(__ => __.Id == model.SourceValue).Name);
              rdetal = rdetail.FirstOrDefault(_ => _.Start <= Decimal.Parse(fi.Value) && _.End >= Decimal.Parse(fi.Value));
              break;
            case 2:
              var rf = _referenceTableService.Get(_ => _.Id == model.SourceValue, _ => _.ReferenceTableDetails);
              var rtb1 = cERefTb(rf, output, fieldsValues);
              rdetal = rdetail.FirstOrDefault(_ => _.Start <= Decimal.Parse(rtb1.Value) && _.End >= Decimal.Parse(rtb1.Value));
              break;
            case 3:
              var form = _formulaService.Get(_ => _.Id == model.SourceValue, _ => _.FormulaDetails);
              //var formu = cFormula(form, fieldsValues, formulas, fields, reftbs);
              var formu = cEFormula(form, output, fieldsValues);
              rdetal = rdetail.FirstOrDefault(_ => _.Start <= formu.Value && _.End >= formu.Value);
              break;
          }
        }
      }
      catch (NullReferenceException e)
      {
        rdetal = null;
      }

      rtb.Name = model.Name;
      if (rdetal != null)
      {
        switch (model.ReturnType)
        {
          case "1":
            rtb.Value = fieldsValues.FirstOrDefault(_ => _.Name == rdetal.Value).Value;
            output.Add(new FormulaElementNCVM
            {
              Name = rdetal.Value,
              Value = rtb.Value,
              Type = 1
            });
            break;
          case "2":
            rtb.Value = cERefTb(_referenceTableService.Get(_ => _.Name == rdetal.Value, _ => _.ReferenceTableDetails), output, fieldsValues).Value;
            output.Add(new FormulaElementNCVM
            {
              Name = rdetal.Value,
              Value = rtb.Value,
              Type = 2
            });
            break;
          case "3":
            rtb.Value = cEFormula(_formulaService.Get(_ => _.Name == rdetal.Value, _ => _.FormulaDetails), output, fieldsValues).Value.ToString();
            output.Add(new FormulaElementNCVM
            {
              Name = rtb.Value,
              Value = rtb.Value,
              Type = 3
            });
            break;
          default:
            rtb.Value = rdetal.Value;
            break;
        }
        rtb.Key = rdetal.Key;
        output.Add(new FormulaElementNCVM
        {
          Name = rtb.Name,
          Value = rtb.Value,
          Type = 2
        });
      }
      else
      {
        rtb.Value = "0";
        rtb.Key = null;
      }

      return rtb;
    }


  }
}
