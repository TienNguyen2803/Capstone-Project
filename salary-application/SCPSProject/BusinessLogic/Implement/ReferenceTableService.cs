using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implement
{
    public class ReferenceTableService : _BaseService<ReferenceTable>, IReferenceTableService
    {
        private readonly IReferenceTableDetailService _referenceTableDetailService;
        private readonly IFieldService _fieldService;
        private readonly IFormulaService _formulaService;
        public ReferenceTableService(IUnitOfWork unitOfWork,
            IReferenceTableDetailService referenceTableDetailService, IFormulaService formulaService, IFieldService fieldService) : base(unitOfWork)
        {
            _referenceTableDetailService = referenceTableDetailService;
            _fieldService = fieldService;
            _formulaService = formulaService;
        }

        //public ReferenceTableService(IUnitOfWork unitOfWork) : base(unitOfWork)
        //{

        //}
        public void Create(ReferenceTable entity, ICollection<ReferenceTableDetail> referenceTableDetails)
        {
            this.Create(entity);
            var list = new List<ReferenceTableDetail>();
            foreach (var detail in referenceTableDetails)
            {
                var refDetail = new ReferenceTableDetail();
                refDetail.ReferenceTableId = entity.Id;
                refDetail.Key = detail.Key;

                switch (entity.ReturnType)
                {
                    case "1":
                        refDetail.Value = _fieldService.Get(_ => _.Name == detail.Value).Id.ToString();
                        break;
                    case "2":
                        refDetail.Value = this.Get(_ => _.Name == detail.Value).Id.ToString();
                        break;
                    case "3":
                        refDetail.Value = _formulaService.Get(_ => _.Name == detail.Value).Id.ToString();
                        break;
                    default:
                        refDetail.Value = detail.Value;
                        break;
                }
                list.Add(refDetail);
            }
            _referenceTableDetailService.Create(list);
        }
    }
}
