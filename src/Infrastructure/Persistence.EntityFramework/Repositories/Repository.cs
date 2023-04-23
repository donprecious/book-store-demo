using System.Linq.Expressions;
using BookStore.Application.Interfaces;
using BookStore.Domain.Common;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;


public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly BookStoreDbContext _context;

    public Repository(BookStoreDbContext context = null)
    {
        _context = context;
    }


    public async Task<T> CreateAsync(T entity)
    {
         await  _context.Set<T>().AddAsync(entity);
         return entity;
    }

    public async Task<List<T>> CreateManyAsync(List<T> collections)
    {
        await  _context.Set<T>().AddRangeAsync(collections);
        return collections;
    }

    public IQueryable<T> Get(Expression<Func<T, bool>> filter)
    {
        var query = _context.Set<T>().Where(filter).AsQueryable();
        return query;
    }
    public IQueryable<T> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        if (filter == null)
        {
            return _context.Set<T>().AsQueryable();
        }
        var result =  _context.Set<T>().Where(filter);
        return result;
    }

    public async Task<T> GetAsync(string id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(filter);
    }

    public async Task RemoveAsync(string id)
    {
        var entity = await GetAsync(id);
        _context.Set<T>().Remove(entity);
    }
    
    public async Task RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
      
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity != null)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        
    }

  
}