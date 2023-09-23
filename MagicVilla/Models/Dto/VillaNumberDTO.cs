using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto;

public class VillaNumberDTO
{
  [Required]
  public required int VillaNo { get; set; }
  public string SpecialDetails { get; set; } = string.Empty;
}
