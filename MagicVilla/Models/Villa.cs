using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models;

public class Villa
{
  [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  [Required, MaxLength(30)]
  public required string Name { get; set; }
  public string Details { get; set; } = string.Empty;
  [Required]
  public double Rate { get; set; }
  public int Sqft { get; set; }
  public int Occupancy { get; set; }
  public string ImageUrl { get; set; } = string.Empty;
  public string Amenity { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public DateTime UpdatedDate { get; set; }
}
