using System.Collections;
using BookStore.Application.Interfaces;
using BookStore.Domain.Common;
using BookStore.Domain.Interface;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IBookStoreContext _context;
    
    private Hashtable _repositories;
    
   
    public UnitOfWork(IBookStoreContext context)
    {
        _context = context;
      
    }
   
    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories == null) _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(Repository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (Repository<TEntity>)_repositories[type];
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken )
    {
        
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
 


 

}