#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using km.Library.Repositories;

namespace km.Translate.DataLib.Data.Models;

public sealed class User : BaseEntity
{
  [JsonIgnore]
  [Required]
  public int UserDetailsId { get; set; }

  [JsonIgnore]
  public int ApprovedPropositionId { get; set; }

  [JsonIgnore]
  public int RoleId { get; set; } = 1;

  [Required]
  public Role Role { get; set; }

  public UserDetails UserDetails { get; set; }

  [JsonIgnore]
  public List<Proposition> ApprovedPropositions { get; set; }
}
