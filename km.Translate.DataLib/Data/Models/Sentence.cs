#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using km.Library.Repositories;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace km.Translate.DataLib.Data.Models;

public sealed class Sentence : BaseEntity
{

  [JsonIgnore]
  public int LanguageVoId { get; set; }

  [Required]
  public string SentenceVo { get; set; }

  [Required]
  public Language LanguageVo { get; set; }

  public ICollection<Proposition> Propositions { get; set; }
}
