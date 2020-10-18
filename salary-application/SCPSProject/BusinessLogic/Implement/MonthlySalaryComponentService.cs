using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
   public class MonthlySalaryComponentService : _BaseService<MonthlySalaryComponent>, IMonthlySalaryComponentService
    {
        public MonthlySalaryComponentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
