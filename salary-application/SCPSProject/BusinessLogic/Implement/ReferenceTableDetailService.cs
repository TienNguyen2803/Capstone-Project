using BusinessLogic.Define;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class ReferenceTableDetailService : _BaseService<ReferenceTableDetail>, IReferenceTableDetailService
    {
        public ReferenceTableDetailService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
