using BookStore.Domain.Common;

namespace BookStore.Domain.Interface;

public interface IUnitOfWork

{
  
    IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> SaveChanges(CancellationToken cancellationToken);
}

