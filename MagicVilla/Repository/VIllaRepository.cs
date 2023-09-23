using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository.IRepository;

namespace MagicVilla.Repository;

public class VIllaRepository : Repository<Villa>, IVillaRepository
{
  private readonly ApplicationDbContext _dbContext;

  public VIllaRepository(ApplicationDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<Villa> UpdateAsync(Villa villa)
  {
    villa.UpdatedDate = DateTime.Now;
    _dbContext.Update(villa);
    await _dbContext.SaveChangesAsync();
    return villa;
  }

}
