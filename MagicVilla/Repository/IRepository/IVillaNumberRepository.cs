using MagicVilla.Models;
using MagicVilla.Repository.IRepository;

namespace MagicVilla;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
  Task<VillaNumber> UpdateAsync(VillaNumber villaNumber);
}
