using BusinessLogic.Define;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class FormulaTypeService : _BaseService<FormulaType>, IFormulaTypeService
    {
        public FormulaTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
