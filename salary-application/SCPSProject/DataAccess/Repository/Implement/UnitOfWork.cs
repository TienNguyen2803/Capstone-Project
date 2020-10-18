namespace DataAccess.Repository.Implement
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Database;
    using DataAccess.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext _dbContext;
        private IDictionary<Type, object> _repository;
        private bool IsDisposed = false;

        public UnitOfWork(IEntityContext context)
        {
            this._dbContext = context.GetContext as DbContext;
            this._repository = new Dictionary<Type, object>();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> repository;
            if (!this._repository.ContainsKey(typeof(T)))
                this._repository.Add(typeof(T), repository = new Repository<T>(_dbContext));
            else
                repository = this._repository[typeof(T)] as Repository<T>;
            return repository;
        }

        public void Save() => this._dbContext.SaveChanges();

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed && disposing)
            {
                this._dbContext.Dispose();
            }
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}

