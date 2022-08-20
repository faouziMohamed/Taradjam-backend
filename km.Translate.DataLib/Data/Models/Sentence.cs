#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using km.Library.Repositories;

namespace km.Translate.DataLib.Data.Models;

public sealed class Sentence : BaseEntity
{

  [JsonIgnore]
  public int SrcLanguageId { get; set; }

  [Required]
  public string SentenceVo { get; set; }

  [Required]
  public Language SrcLanguage { get; set; }

  public ICollection<Proposition> Propositions { get; set; }
}
