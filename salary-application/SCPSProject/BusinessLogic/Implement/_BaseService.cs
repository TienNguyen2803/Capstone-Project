namespace BusinessLogic.Implement
{
    using DataAccess.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;

    public abstract class _BaseService<T> where T : class
    {
        protected readonly IUnitOfWork iUnitOfWork;
        protected IRepository<T> iRepository;

        public _BaseService(IUnitOfWork unitOfWork)
        {
            this.iUnitOfWork = unitOfWork;
            this.iRepository = unitOfWork.GetRepository<T>();
        }

        public virtual void Create(T entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    this.iRepository.Create(entity);
                    this.iUnitOfWork.Save();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Create(ICollection<T> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var item in entity)
                    {
                        this.iRepository.Create(item);
                    }
                    this.iUnitOfWork.Save();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    this.iRepository.Update(entity);
                    this.iUnitOfWork.Save();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Update(ICollection<T> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var item in entity)
                    {
                        this.iRepository.Update(item);
                    }
                    this.iUnitOfWork.Save();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public virtual void Delete(T entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    this.iRepository.Delete(entity);
                    this.iUnitOfWork.Save();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Delete(ICollection<T> entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var item in entity)
                    {
                        this.iRepository.Delete(item);
                        this.iUnitOfWork.Save();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual T Get(Expression<Func<T, bool>> predict, params Expression<Func<T, object>>[] includes)
            => this.iRepository.Get(predict, includes);

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
            => this.iRepository.GetAll(includes);
    }
}
