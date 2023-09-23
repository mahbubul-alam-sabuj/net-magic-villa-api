using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto;

public class VillaDTO
{
  public int Id { get; set; }
  [Required, MaxLength(30)]
  public required string Name { get; set; }
  public string Details { get; set; } = string.Empty;
  [Required]
  public required double Rate { get; set; }
  public int Sqft { get; set; }
  public int Occupancy { get; set; }
  public string ImageUrl { get; set; } = string.Empty;
  public string Amenity { get; set; } = string.Empty;
}
