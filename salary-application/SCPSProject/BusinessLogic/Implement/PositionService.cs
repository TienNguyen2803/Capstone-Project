using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class PositionService : _BaseService<Position>, IPositionService
    {
        public PositionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
