using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Implement
{
    public class FormulaService : _BaseService<Formula>, IFormulaService
    {
        private readonly IFormulaDetailService _formulaDetailService;
        private readonly IDocumentService _documentService;

        public FormulaService(IUnitOfWork unitOfWork, IFormulaDetailService formulaDetailService, IDocumentService documentService) : base(unitOfWork)
        {
            _formulaDetailService = formulaDetailService;
            _documentService = documentService;
        }
        public void Create(Formula entity, ICollection<FormulaDetail> formulaDetails, int docId)
        {
            this.Create(entity);
            if (entity.IsSalaryFormula)
            {
                var doc = _documentService.Get(_ => _.Id == docId);
                var formula = this.Get(_ => _.Name == entity.Name);
                doc.FormulaId = formula.Id;
                _documentService.Update(doc);
            }
            foreach (var detail in formulaDetails)
            {
                detail.FormulaId = entity.Id;
                _formulaDetailService.Create(detail);
                _formulaDetailService.CreateType(detail.Type, detail.Id, detail.RefId, detail.Value);
            };
        }

        //public ICollection<Formula> GetAll()
        //{
        //    ICollection<Formula> result = new List<Formula>();
        //    var listFormula = this.GetAll().ToList();
        //    //result = listFormula;

        //    return result;
        //}
    }
}
