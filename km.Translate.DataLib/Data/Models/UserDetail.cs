using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using km.Library.Repositories;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedMember.Global

#pragma warning disable CS8618

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace km.Translate.DataLib.Data.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class UserDetails : BaseEntity
{
  [Required]
  public string Email { get; set; }

  [Required]
  [StringLength(100)]
  public string Username { get; set; }

  [Required]
  [StringLength(200)]
  [JsonIgnore]
  public string Password { get; set; }

  [JsonIgnore]
  public bool AccountVerified { get; set; } = false;
}
