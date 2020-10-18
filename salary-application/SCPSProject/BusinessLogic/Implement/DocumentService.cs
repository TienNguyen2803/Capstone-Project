using BusinessLogic.Define;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Implement
{
    public class DocumentService : _BaseService<Document>, IDocumentService
    {
        public DocumentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
