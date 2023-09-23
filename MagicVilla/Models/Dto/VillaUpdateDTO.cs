using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto;

public class VillaUpdateDTO
{
  [Required]
  public int Id { get; set; }
  [Required, MaxLength(30)]
  public required string Name { get; set; }
  public string Details { get; set; } = string.Empty;
  [Required]
  public required double Rate { get; set; }
  [Required]
  public int Sqft { get; set; }
  [Required]
  public int Occupancy { get; set; }
  [Required]
  public required string ImageUrl { get; set; }
  public string Amenity { get; set; } = string.Empty;
}
