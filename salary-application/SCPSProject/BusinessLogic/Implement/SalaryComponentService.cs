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
    public class SalaryComponentService : _BaseService<SalaryComponent>, ISalaryComponentService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPositionDetailService _positionDetailService;
        private readonly IFieldService _fieldlService;
        public SalaryComponentService(IUnitOfWork unitOfWork, IEmployeeService employeeService, 
            IPositionDetailService positionDetailService, IFieldService fieldlService) : base(unitOfWork)
        {
            _employeeService = employeeService;
            _positionDetailService = positionDetailService;
            _fieldlService = fieldlService;
        }

    }
}
