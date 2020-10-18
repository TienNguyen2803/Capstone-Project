using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneUI.ViewModels
{
  public class _ModelMapping
  {
    #region Field
    public Field ConvertToModel(FieldCreateVM viewModel)
            => new Field
            {
              Name = viewModel.Name,
              LongName = viewModel.LongName,
              DataType = viewModel.DataType,
              IsMonthlyComponent = viewModel.IsMonthlySalaryComponent,
              Description = viewModel.Description
            };
    public FiellExelVM ConvertToExelViewModel(Field model)
      => new FiellExelVM
      {
        Field = "Field_" + model.Name,
      };
    public ICollection<FiellExelVM> ConvertToExelViewModel(ICollection<Field> model)
           => model.Select(m => ConvertToExelViewModel(m)).ToList();
    public FieldVM ConvertToViewModel(Field model)
      => new FieldVM
      {
        Id = model.Id,
        Name = model.Name,
        LongName = model.LongName,
        DataType = model.DataType,
        Status = model.Status,
        CellMapping = model.CellMapping,
        IsMonthlyComponent = model.IsMonthlyComponent,
        Description = model.Description
      };

    public ICollection<FieldVM> ConvertToViewModel(ICollection<Field> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();

    public FormulaDetailGetVM ConvertToElementViewModel(Field model)
      => new FormulaDetailGetVM
      {
        Id = model.Id,
        Name = model.Name,
        Type = 1
      };

    public ICollection<FormulaDetailGetVM> ConvertToElementViewModel(ICollection<Field> model)
           => model.Select(m => ConvertToElementViewModel(m)).ToList();

    #endregion

    #region ReferenceTable
    public ReferenceTable ConvertToModel(ReferenceTableCreateVM viewModel)
      => new ReferenceTable
      {
        Name = viewModel.Name,
        SourceValue = viewModel.SourceValue,
        CompareType = viewModel.CompareType,
        ReturnType = viewModel.ReturnType,
        SourceType = viewModel.SourceType,
        Description = viewModel.Description
      };

    public ReferenceTableVM ConvertToViewModel(ReferenceTable model)
      => new ReferenceTableVM
      {
        Name = model.Name,
        SourceValue = model.SourceValue,
        ReturnType = model.ReturnType,
        SourceType = model.SourceType,
        Description = model.Description,
        ReferenceTableDetailVMs = ConvertToViewModel(model.ReferenceTableDetails)
      };
    public ICollection<ReferenceTableVM> ConvertToViewModel(ICollection<ReferenceTable> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();

    public FormulaDetailGetVM ConvertToElementViewModel(ReferenceTable model)
      => new FormulaDetailGetVM
      {
        Id = model.Id,
        Name = model.Name,
        Type = 2
      };

    public ICollection<FormulaDetailGetVM> ConvertToElementViewModel(ICollection<ReferenceTable> model)
           => model.Select(m => ConvertToElementViewModel(m)).ToList();
    #endregion

    #region ReferenceTableDetail
    public ReferenceTableDetail ConvertToModel(ReferenceTableDetailCreateVM viewModel)
      => new ReferenceTableDetail
      {
        Key = viewModel.Key,
        Value = viewModel.Value
      };
    public ICollection<ReferenceTableDetail> ConvertToModel(ICollection<ReferenceTableDetailCreateVM> model)
            => model.Select(m => ConvertToModel(m)).ToList();

    public ReferenceTableDetailVM ConvertToViewModel(ReferenceTableDetail model)
    => new ReferenceTableDetailVM
    {
      Key = model.Key,
      Value = model.Value
    };


    public ICollection<ReferenceTableDetailVM> ConvertToViewModel(ICollection<ReferenceTableDetail> model)
            => model.Select(m => ConvertToViewModel(m)).ToList();


    #endregion

    #region Formula
    public FiellExelVM ConvertToExelViewModel(Formula model)
      => new FiellExelVM
      {
        Field = "Formula_" + model.Name,
      };
    public ICollection<FiellExelVM> ConvertToExelViewModel(ICollection<Formula> model)
           => model.Select(m => ConvertToExelViewModel(m)).ToList();
    public FormulaDetailGetVM ConvertToElementViewModel(Formula model)
      => new FormulaDetailGetVM
      {
        Id = model.Id,
        Name = model.Name,
        Type = 3
      };

    public ICollection<FormulaDetailGetVM> ConvertToElementViewModel(ICollection<Formula> model)
           => model.Select(m => ConvertToElementViewModel(m)).ToList();
    public Formula ConvertToModel(FormulaCreateVM viewModel)
     => new Formula
     {
       Name = viewModel.Name,
       CreateDate = DateTimeOffset.Now,
       Type = viewModel.Type,
       IsSalaryFormula = viewModel.IsSalaryFormula,
       Description = viewModel.Description
     };


    public FormulaVM ConvertToViewModel(Formula model)
      => new FormulaVM
      {
        Id = model.Id,
        Name = model.Name,
        CreateDate = model.CreateDate,
        Type = model.IsSalaryFormula ? "CT Lương" : "CT Thành phần",
        Description = model.Description,
      };

    public Formula ConvertToModel(FormulaNCVM viewModel)
     => new Formula
     {
       Name = viewModel.Name,
       CreateDate = DateTimeOffset.Now,
       IsSalaryFormula = viewModel.IsSalaryFormula,
       Description = viewModel.Description
     };

    public FormulaDocVM ConvertToFDViewModel(Formula model)
      => new FormulaDocVM
      {
        Id = model.Id,
        Name = model.Name,
        CreateDate = model.CreateDate,
        Type = model.IsSalaryFormula ? "CT Lương" : "CT Thành phần",
        Description = model.Description,

      };

    public FormulaReturnVM ConvertToReturnViewModel(FormulaVM model)
      => new FormulaReturnVM
      {
        Id = model.Id,
        Name = model.Formula
      };

    public FieldCVM ConvertToFCViewModel(Field model)
     => new FieldCVM
     {
       Name = model.Name
     };

    public ICollection<FieldCVM> ConvertToFCViewModel(ICollection<Field> model)
          => model.Select(m => ConvertToFCViewModel(m)).ToList();

    public ICollection<FormulaVM> ConvertToViewModel(ICollection<Formula> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();
    #endregion

    #region Document

    public Document ConvertToModel(DocumentCreateVM viewModel)
           => new Document
           {
             Code = viewModel.Code,
             SignDate = viewModel.SignDate,
             ApplyDate = viewModel.ApplyDate,
             Deadline = viewModel.Deadline,
             Description = viewModel.Description,
             CloseDay = viewModel.CloseDay,
           };

    public DocumentVM ConvertToViewModel(Document model)
      => new DocumentVM
      {
        Id = model.Id,
        Code = model.Code,
        SignDate = model.SignDate,
        ApplyDate = model.ApplyDate,
        Description = model.Description,
        CloseDay = model.CloseDay,
        Deadline = model.Deadline,
        DocumentUrl = model.DocumentUrl,
      };

    public ICollection<DocumentVM> ConvertToViewModel(ICollection<Document> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();
    #endregion

    #region Employee
    public Employee ConvertToModel(EmployeeCreateVM viewModel)
      => new Employee
      {
        Code = viewModel.Code,
        Fullname = viewModel.Fullname,
        DateOfBirth = viewModel.DateOfBirth,
        Address = viewModel.Address,
        DepartmentId = viewModel.DepartmentId,
        Email = viewModel.Email,
        Gender = viewModel.Gender,
        IsForeigner = viewModel.IsForeigner,
        Phone = viewModel.Phone,
        StartDate = viewModel.StartDate
      };

    public EmployeeCreateVM ConvertToViewModel(Employee model)
      => new EmployeeCreateVM
      {
        Code = model.Code,
        Fullname = model.Fullname,
        DateOfBirth = model.DateOfBirth.Value,
        Address = model.Address,
        DepartmentId = model.DepartmentId,
        Email = model.Email,
        Gender = model.Gender,
        IsForeigner = model.IsForeigner.Value,
        Phone = model.Phone,
        StartDate = model.StartDate.Value,
        PositionDetailCreateVM = new PositionDetailCreateVM
        {
          PositionId = model.PositionDetails.OrderByDescending(_ => _.ApplyDate).FirstOrDefault().Id,
          ApplyDate = model.PositionDetails.OrderByDescending(_ => _.ApplyDate).FirstOrDefault().ApplyDate.Value
        }
      };

    public ICollection<EmployeeCreateVM> ConvertToViewModel(ICollection<Employee> model)
          => model.Select(m => ConvertToViewModel(m)).ToList();

    #endregion

    #region PositionDetail
    public PositionDetail ConvertToModel(PositionDetailCreateVM viewModel)
      => new PositionDetail
      {
        PositionId = viewModel.PositionId,
        ApplyDate = viewModel.ApplyDate
      };
    #endregion

    #region FormulaDetail
    public FieldType ConvertToFieldTypeModel(FormulaDetailTypeCreateVM viewModel)
           => new FieldType
           {
             FieldId = viewModel.Id
           };

    public ReferenceTableType ConvertToRefTypeModel(FormulaDetailTypeCreateVM viewModel)
           => new ReferenceTableType
           {
             RefenceTableTypeId = viewModel.Id
           };

    public FormulaType ConvertToFormulaTypeModel(FormulaDetailTypeCreateVM viewModel)
           => new FormulaType
           {
             FormulaId = viewModel.Id
           };

    public ConstantType ConvertToConstantTypeModel(FormulaDetailTypeCreateVM viewModel)
           => new ConstantType
           {
             Value = viewModel.Value
           };

    public FormulaDetail ConvertToModel(FormulaDetailCreateVM viewModel)
      => new FormulaDetail
      {
        Type = viewModel.Type,
        Ordinal = viewModel.Ordinal,
        Operator = viewModel.Operator,
        RefId = viewModel.FDType.Id,
        Value = viewModel.FDType.Value,
      };
    public ICollection<FormulaDetail> ConvertToModel(ICollection<FormulaDetailCreateVM> model)
            => model.Select(m => ConvertToModel(m)).ToList();



    public FormulaDetailVM ConvertToViewModel(FormulaDetail model)
    => new FormulaDetailVM
    {
      Operator = model.Operator,
      Type = model.Type,
      Id = model.Id,
      //ConstantTypeVM = ConvertToViewModel(model.ConstantType),
      //FieldTypeVM = ConvertToViewModel(model.FieldType),
      //RefTableTypeVM = ConvertToViewModel(model.ReferenceTableType),
      //FormulaTypeVM = ConvertToViewModel(model.FormulaType)
    };



    public ICollection<FormulaDetailVM> ConvertToViewModel(ICollection<FormulaDetail> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();
    #endregion

    #region formuladetailtype
    public FieldTypeVM ConvertToViewModel(FieldType model)
      => new FieldTypeVM
      {
        Name = model.Field.Name
      };
    public RefTableTypeVM ConvertToViewModel(ReferenceTableType model)
      => new RefTableTypeVM
      {
        Name = model.ReferenceTable.Name
      };
    public FormulaTypeVM ConvertToViewModel(FormulaType model)
      => new FormulaTypeVM
      {
        Name = model.Formula.Name
      };
    public ConstantTypeVM ConvertToViewModel(ConstantType model)
      => new ConstantTypeVM
      {
        Value = model.Value
      };


    #endregion

    #region Payroll
    public Payroll ConvertToModel(PayrollCreateVM viewModel)
      => new Payroll
      {
        FromDate = viewModel.FromDate,
        ToDate = viewModel.ToDate,
        PayDate = viewModel.PayDate,
        Month = viewModel.Month,
        Year = viewModel.Year,
        Revenue = viewModel.Revenue,
        StandardWorkDay = viewModel.StandardWorkDay,
        Status = PayrollStatus.New
      };

    public PayrollVM ConvertToViewModel(Payroll model)
      => new PayrollVM
      {
        FromDate = model.FromDate,
        ToDate = model.ToDate,
        PayDate = model.PayDate,
        Month = model.Month,
        Year = model.Year,
        Revenue = model.Revenue,
        StandardWorkDay = model.StandardWorkDay,
        Id = model.Id,
        Status = model.Status,
        DocumentVM = new DocumentVM
        {
          Code = model.Document.Code
        }
      };
    public ICollection<PayrollVM> ConvertToViewModel(ICollection<Payroll> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();

#endregion

#region Salarycomponent
    public SalaryComponent ConvertToModel(SalaryComponentVM viewModel)
            => new SalaryComponent
            {
              EmpId = viewModel.EmpId,
              FieldId = viewModel.FieldId,
              Value = viewModel.Value,
              ApplyDate = DateTimeOffset.Now
            };

    public ICollection<SalaryComponent> ConvertToModel(ICollection<SalaryComponentVM> viewModel)
      => viewModel.Select(m => ConvertToModel(m)).ToList();

    public SalaryComponentCreateVM ConvertToCreateViewModel(SalaryComponent model)
            => new SalaryComponentCreateVM
            {
              EmpId = model.EmpId,
              FieldId = model.FieldId,
              Value = model.Value,
            };

    public ICollection<SalaryComponentCreateVM> ConvertToCreateViewModel(ICollection<SalaryComponent> model)
      => model.Select(m => ConvertToCreateViewModel(m)).ToList();

    public SalaryComponentCreateVM ConvertToViewModel(SalaryComponentVM model)
            => new SalaryComponentCreateVM
            {
              EmpId = model.EmpId,
              FieldId = model.FieldId,
              Value = model.Value,
            };

    public ICollection<SalaryComponentCreateVM> ConvertToViewModel(ICollection<SalaryComponentVM> model)
      => model.Select(m => ConvertToViewModel(m)).ToList();
#endregion
#region PayslipTemplate
    public PaySlipTemplateVM ConvertToViewModel(PayslipTemplate model)
         => new PaySlipTemplateVM
         {
           TemplateUrl = model.TemplateUrl.Split("\\")[3].Split(".")[0],
           Status = model.Status
         };

    public ICollection<PaySlipTemplateVM> ConvertToViewModel(ICollection<PayslipTemplate> model)
           => model.Select(m => ConvertToViewModel(m)).ToList();
#endregion
  }
}
