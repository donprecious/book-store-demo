using System.Linq.Expressions;
using BookStore.Domain.Common;

namespace BookStore.Domain.Interface;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity);
    Task<List<T>> CreateManyAsync(List<T> collections);
    
    

    IQueryable<T> GetAllAsync(Expression<Func<T, bool>> filter=null);
    IQueryable<T> Get(Expression<Func<T, bool>> filter);
    Task<T> GetAsync(string id);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task RemoveAsync(string id);
   Task RemoveAsync(T entity);
    Task UpdateAsync(T entity);
   

}