using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class EmployeeService : _BaseService<Employee>, IEmployeeService
    {
        private readonly IAccountService _accountService;
        private readonly IPositionDetailService _positionDetailService;
        public EmployeeService(IUnitOfWork unitOfWork, IAccountService accountService,
            IPositionDetailService positionDetailService) : base(unitOfWork)
        {
            _accountService = accountService;
            _positionDetailService = positionDetailService;
        }

        public void Create(Employee entity, PositionDetail positionDetail)
        {
            
            this.Create(entity);

            Account account = new Account();
            account.Code = entity.Code;
            account.Password = "qwerty@123";
            _accountService.Create(account);

            positionDetail.EmpCode = entity.Code;
            _positionDetailService.Create(positionDetail);

            
        }
    }
}
