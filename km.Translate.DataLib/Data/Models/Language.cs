#nullable disable
using System.ComponentModel.DataAnnotations;
using km.Library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace km.Translate.DataLib.Data.Models;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(LongName), IsUnique = true)]
[Index(nameof(ShortName), IsUnique = true)]
public sealed class Language : BaseEntity
{
  [Required]
  [StringLength(30)]
  public string Name { get; set; }

  [Required]
  [StringLength(20)]
  public string LongName { get; set; }

  [Required]
  [StringLength(10)]
  public string ShortName { get; set; }
}
