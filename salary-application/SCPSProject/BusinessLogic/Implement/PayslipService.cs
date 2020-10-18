using BusinessLogic.Define;
using BusinessLogic.Utils;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Implement
{
    public class PayslipService : _BaseService<Payslip>, IPayslipService
    {
        private readonly IFieldTypeService _fieldTypeService;
        private readonly IFormulaTypeService _formulaTypeService;
        private readonly IFormulaService _formulaService;
        private readonly IReferenceTableTypeService _referenceTableTypeService;
        private readonly IConstantTypeService _constantTypeService;
        private readonly IFieldService _fieldService;
        private readonly IReferenceTableDetailService _referenceTableDetailService;
        private readonly IReferenceTableService _referenceTableService;

        public PayslipService(IUnitOfWork unitOfWork, IConstantTypeService constantTypeService,
      IFormulaTypeService formulaTypeService, IFieldTypeService fieldTypeService, IReferenceTableTypeService referenceTableTypeService,
      IFieldService fieldService, IReferenceTableService referenceTableService,
      IReferenceTableDetailService referenceTableDetailService, FormulaService formulaService) : base(unitOfWork)
        {
            _constantTypeService = constantTypeService;
            _formulaTypeService = formulaTypeService;
            _formulaService = formulaService;
            _fieldTypeService = fieldTypeService;
            _referenceTableTypeService = referenceTableTypeService;
            _fieldService = fieldService;
            _referenceTableService = referenceTableService;
            _referenceTableDetailService = referenceTableDetailService;
        }

        public void Create(int payrollId, string employeeId)
        {
            var payslip = new Payslip();
            payslip.PayrollId = payrollId;
            payslip.EmpId = employeeId;
            this.Create(payslip);
        }

        public FormulaTypeSSVM GetFormula(Formula formula, int payslipId, string employeeId)
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
                detail.Type = fDetail.Type;

                switch (fDetail.Type)
                {
                    case 1:
                        var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
                        var field = _fieldService.GetField(fieldType.FieldId, payslipId, employeeId);
                        detail.FieldType = new RefTypeSSVM();
                        detail.FieldType.Name = field.Name;
                        if (field.Value == "N/A")
                        {
                            if (fDetail.Operator == 3 || fDetail.Operator == 4)
                            {
                                detail.FieldType.Value = "1";
                                break;
                            }
                            else
                            {
                                detail.FieldType.Value = "0";
                                break;
                            }
                        }
                        detail.FieldType.Value = field.Value;
                        break;
                    case 2:
                        var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.ReferenceTable.ReferenceTableDetails);
                        detail.RefType = GetRefTable(refType.ReferenceTable, payslipId, employeeId);
                        break;
                    case 3:
                        var formular = _formulaTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.Formula.FormulaDetails);
                        detail.FormulaType = GetFormula(formular.Formula, payslipId, employeeId);
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
                    result.Value = result.Details.Min(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 3:
                    result.Value = result.Details.Max(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 4:
                    result.Value = result.Details.Average(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 5:
                    result.Value = result.Details.Max(_ => Decimal.Parse(_.Value)) - result.Details.Min(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
            }

            return result;
        }
        public FormulaTypeSSVM GetFormulaV2(Formula formula, int payslipId, string employeeId)
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
                detail.Type = fDetail.Type;

                switch (fDetail.Type)
                {
                    case 1:
                        var fieldType = _fieldTypeService.Get(_ => _.FormulaDetailId == fDetail.Id);
                        var field = _fieldService.GetField(fieldType.FieldId, payslipId, employeeId);
                        detail.FieldType = new RefTypeSSVM();
                        detail.FieldType.Name = field.LongName;
                        if (field.Value == "N/A")
                        {
                            if (fDetail.Operator == 3 || fDetail.Operator == 4)
                            {
                                detail.FieldType.Value = "1";
                                break;
                            }
                            else
                            {
                                detail.FieldType.Value = "0";
                                break;
                            }
                        }
                        detail.FieldType.Value = field.Value;
                        break;
                    case 2:
                        var refType = _referenceTableTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.ReferenceTable.ReferenceTableDetails);
                        detail.RefType = GetRefTable(refType.ReferenceTable, payslipId, employeeId);
                        break;
                    case 3:
                        var formular = _formulaTypeService.Get(_ => _.FormulaDetailId == fDetail.Id, _ => _.Formula.FormulaDetails);
                        detail.FormulaType = GetFormulaV2(formular.Formula, payslipId, employeeId);
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
                    result.Value = result.Details.Min(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 3:
                    result.Value = result.Details.Max(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 4:
                    result.Value = result.Details.Average(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
                case 5:
                    result.Value = result.Details.Max(_ => Decimal.Parse(_.Value)) - result.Details.Min(_ => Decimal.Parse(_.Value));
                    result.Formula = result.Name;
                    break;
            }

            return result;
        }
        public RefTypeSSVM GetRefTable(ReferenceTable referenceTable, int payslipId, string employeeId)
        {
            var detail = new RefTypeSSVM();
            var refef = new ReferenceTableDetail();
            var refer = new ReferenceTableDetailCreateVM();
            if (referenceTable.CompareType == 1)
            {
                switch (referenceTable.SourceType)
                {
                    default:
                        var fi = _fieldService.GetField(referenceTable.SourceValue, payslipId, employeeId);
                        refef = _referenceTableDetailService.Get(_ => _.Key == fi.Value);
                        break;
                    case 2:
                        var reftbl = _referenceTableService.Get(_ => _.Id == referenceTable.SourceValue, _ => _.ReferenceTableTypes, _ => _.ReferenceTableDetails);
                        var reftb = GetRefTable(reftbl, payslipId, employeeId);
                        refef = _referenceTableDetailService.Get(_ => _.Key == reftb.Value);
                        break;
                    case 3:
                        var form = _formulaService.Get(_ => _.Id == referenceTable.SourceValue, _ => _.FormulaDetails);
                        var formu = GetFormula(form, payslipId, employeeId);
                        refef = _referenceTableDetailService.Get(_ => _.Key == formu.Value.ToString());
                        break;
                }
            }
            else
            {
                var rdetail = referenceTable.ReferenceTableDetails.Select(rd => new ReferenceTableDetailCreateVM
                {
                    Key = rd.Key,
                    Value = rd.Value,
                    Start = Decimal.Parse(rd.Key.Split(",")[0].Trim()),
                    End = Decimal.Parse(rd.Key.Split(",")[1].Trim())
                });
                switch (referenceTable.SourceType)
                {
                    default:
                        var fi = _fieldService.GetField(referenceTable.SourceValue, payslipId, employeeId);
                        if (fi.Value != "N/A")
                        {
                            refer = rdetail.FirstOrDefault(_ => _.Start <= Decimal.Parse(fi.Value) && _.End >= Decimal.Parse(fi.Value));
                            refef = new ReferenceTableDetail
                            {
                                Key = refer.Key,
                                Value = refer.Value
                            };
                        } else
                        {
                            refef = null;
                        }
                            detail.SourceName = fi.Name;
                            break;
                    case 2:
                        var reftbl = _referenceTableService.Get(_ => _.Id == referenceTable.SourceValue, _ => _.ReferenceTableTypes, _ => _.ReferenceTableDetails);
                        var reftb = GetRefTable(reftbl, payslipId, employeeId);
                        refer = rdetail.FirstOrDefault(_ => _.Start <= Decimal.Parse(reftb.Value) && _.End >= Decimal.Parse(reftb.Value));
                        refef = new ReferenceTableDetail
                        {
                            Key = refer.Key,
                            Value = refer.Value
                        };
                        detail.SourceName = reftbl.Name;
                        break;
                    case 3:
                        var form = _formulaService.Get(_ => _.Id == referenceTable.SourceValue, _ => _.FormulaDetails);
                        var formu = GetFormula(form, payslipId, employeeId);
                        refer = rdetail.FirstOrDefault(_ => _.Start <= formu.Value && _.End >= formu.Value);
                        refef = new ReferenceTableDetail
                        {
                            Key = refer.Key,
                            Value = refer.Value
                        };
                        detail.SourceName = form.Name;
                        break;
                }
            }

            if (refef != null)
            {
                detail.Name = referenceTable.Name;
                //detail.Value = refef.Value;
                switch (referenceTable.ReturnType)
                {
                    case "1":
                        var fi2 = _fieldService.GetField(Int32.Parse(refef.Value), payslipId, employeeId);
                        detail.Value = fi2.Value;
                        break;
                    case "2":
                        var reftbl2 = _referenceTableService.Get(_ => _.Id == Int32.Parse(refef.Value));
                        var reftb2 = GetRefTable(reftbl2, payslipId, employeeId);
                        detail.Value = reftb2.Value;
                        break;
                    case "3":
                        var form2 = _formulaService.Get(_ => _.Id == Int32.Parse(refef.Value));
                        var formu2 = GetFormula(form2, payslipId, employeeId);
                        detail.Value = formu2.Value.ToString();
                        break;
                    default:
                        detail.Value = refef.Value;
                        break;
                }
                detail.Key = detail.SourceName + ": " + refef.Key;
            }
            else
            {
                detail = new RefTypeSSVM();
                detail.Name = referenceTable.Name;
                detail.Value = "0";
            }
            return detail;
        }
    }


    public class FormulaTypeSSVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FormulaDetailSSVM> Details { get; set; }
        public decimal Value { get; set; }
        public string Formula { get; set; }
    }

    public class FormulaDetailSSVM
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int Ordinal { get; set; }
        public string Value { get; set; }
        public int Operator { get; set; }
        public ConstantTypeSSVM ConstantType { get; set; }
        public RefTypeSSVM RefType { get; set; }
        public RefTypeSSVM FieldType { get; set; }
        public FormulaTypeSSVM FormulaType { get; set; }
    }

    public class ConstantTypeSSVM
    {
        public decimal Value { get; set; }
    }

    public class RefTypeSSVM
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
        public string SourceName { get; set; }
    }
    public class ReferenceTableDetailCreateVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public decimal Start { get; set; }
        public decimal End { get; set; }
    }
}
