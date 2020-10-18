using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class ConstantTypeService : _BaseService<ConstantType>, IConstantTypeService
    {
        public ConstantTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
