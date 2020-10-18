using BusinessLogic.Define;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class ReferenceTableTypeService : _BaseService<ReferenceTableType>, IReferenceTableTypeService
    {
        public ReferenceTableTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
