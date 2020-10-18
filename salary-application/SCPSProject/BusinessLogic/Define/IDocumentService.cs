namespace BusinessLogic.Define
{
    using DataAccess;
    using DataAccess.Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IDocumentService
    {
        void Create(Document entity);
        void Update(Document entity);
        void Delete(Document entity);
        Document Get(Expression<Func<Document, bool>> predicated, params Expression<Func<Document, object>>[] includes);
        IQueryable<Document> GetAll(params Expression<Func<Document, object>>[] includes);
    }
}
