﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models;

public class VillaNumber
{
  [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
  public int VillaNo { get; set; }
  public string SpecialDetails { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public DateTime UpdatedDate { get; set; }
}