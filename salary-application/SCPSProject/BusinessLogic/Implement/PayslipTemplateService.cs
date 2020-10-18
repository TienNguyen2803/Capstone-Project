using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class PayslipTemplateService : _BaseService<PayslipTemplate>, IPayslipTemplateService
    {
        public PayslipTemplateService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public void CreatePayslipTemplate(PayslipTemplate entity)
        {
            throw new NotImplementedException();
        }
    }
}
