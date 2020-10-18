using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class FormulaDetailService : _BaseService<FormulaDetail>, IFormulaDetailService
    {
        private readonly IFieldTypeService _fieldTypeServie;
        private readonly IReferenceTableTypeService _referenceTableTypeService;
        private readonly IFormulaTypeService _formulaTypeService;
        private readonly IConstantTypeService _constantTypeService;
        public FormulaDetailService(IUnitOfWork unitOfWork, IFieldTypeService fieldTypeServie,
            IReferenceTableTypeService referenceTableTypeService, IFormulaTypeService formulaTypeService, IConstantTypeService constantTypeService
            ) : base(unitOfWork)
        {
            _fieldTypeServie = fieldTypeServie;
            _referenceTableTypeService = referenceTableTypeService;
            _formulaTypeService = formulaTypeService;
            _constantTypeService = constantTypeService;
        }

        public void CreateType(int type, int formularDetailId, int refId, decimal value)
        {
            switch (type)
            {
                case 1:
                    _fieldTypeServie.Create(new FieldType
                    {
                        FormulaDetailId = formularDetailId,
                        FieldId = refId
                    });
                    break;
                case 2:
                    _referenceTableTypeService.Create(new ReferenceTableType
                    {
                        FormulaDetailId = formularDetailId,
                        RefenceTableTypeId = refId
                    });
                    break;
                case 3:
                    _formulaTypeService.Create(new FormulaType
                    {
                        FormulaDetailId = formularDetailId,
                        FormulaId = refId
                    });
                    break;
                case 4:
                    _constantTypeService.Create(new ConstantType
                    {
                        FormulaDetailId = formularDetailId,
                        Value = value
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
