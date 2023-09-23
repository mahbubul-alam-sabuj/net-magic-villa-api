using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto;

public class VillaNumberUpdateDTO
{
  [Required]
  public required int VillaNo { get; set; }
  public string SpecialDetails { get; set; } = string.Empty;
}
