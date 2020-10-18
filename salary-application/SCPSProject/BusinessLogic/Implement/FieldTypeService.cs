using BusinessLogic.Define;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class FieldTypeService : _BaseService<FieldType>, IFieldTypeService
    {
        public FieldTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
