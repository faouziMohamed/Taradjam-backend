#nullable disable
using System.ComponentModel.DataAnnotations;
using km.Library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace km.Translate.DataLib.Data.Models;

[Index(nameof(LanguageName), nameof(LanguageShortName), IsUnique = true)]
public sealed class Language : BaseEntity
{
  [Required]
  [StringLength(20)]
  public string LanguageName { get; set; }

  [StringLength(10)]
  public string LanguageShortName { get; set; }
}
