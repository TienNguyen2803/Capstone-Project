using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
   public class PayrollComponentService : _BaseService<PayrollComponent>, IPayrollComponentService
    {
        public PayrollComponentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
