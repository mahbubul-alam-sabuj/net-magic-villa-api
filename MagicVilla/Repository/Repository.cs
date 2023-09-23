using System.Linq.Expressions;
using MagicVilla.Data;
using MagicVilla.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Repository;

public class Repository<T> : IRepository<T> where T : class
{
  private readonly ApplicationDbContext _dbContext;
  private readonly DbSet<T> _dbSet;

  public Repository(ApplicationDbContext dbContext)
  {
    _dbContext = dbContext;
    _dbSet = _dbContext.Set<T>();
  }
  public async Task CreateAsync(T entity)
  {
    await _dbSet.AddAsync(entity);
    await SaveAsync();
  }

  public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter, bool tracked = true)
  {
    IQueryable<T> entities = _dbSet;
    if (!tracked)
    {
      entities = entities.AsNoTracking();
    }
    if (filter != null)
    {
      entities = entities.Where(filter);
    }
    return await entities.FirstOrDefaultAsync();
  }

  public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter)
  {
    IQueryable<T> entities = _dbSet;
    if (filter != null)
    {
      entities = entities.Where(filter);
    }
    return await entities.ToListAsync();
  }

  public async Task RemoveAsync(T entity)
  {
    _dbSet.Remove(entity);
    await SaveAsync();
  }

  public async Task SaveAsync()
  {
    await _dbContext.SaveChangesAsync();
  }
}
