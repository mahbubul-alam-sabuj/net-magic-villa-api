using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository;

namespace MagicVilla;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
  private readonly ApplicationDbContext _dbContext;
  public VillaNumberRepository(ApplicationDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<VillaNumber> UpdateAsync(VillaNumber villaNumber)
  {
    villaNumber.UpdatedDate = DateTime.Now;
    _dbContext.Update(villaNumber);
    await _dbContext.SaveChangesAsync();
    return villaNumber;
  }
}
