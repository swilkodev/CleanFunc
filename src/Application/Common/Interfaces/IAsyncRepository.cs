using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CleanFunc.Domain.Entities;

namespace CleanFunc.Application.Common.Interfaces
{
    public interface IAsyncRepository<T, TId> where T : BaseEntity<TId>
    {

        Task<T> GetById(TId id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);

        Task Add(IEnumerable<T> records);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);

    }
}