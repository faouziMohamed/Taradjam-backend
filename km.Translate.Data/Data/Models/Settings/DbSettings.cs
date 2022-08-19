// ReSharper disable PropertyCanBeMadeInitOnly.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace km.Translate.Data.Data.Models.Settings;

public sealed class DbSettings
{
  public string DefaultSentencesFile { get; init; } = null!;
  public string LocaleShort { get; init; } = null!;
  public string LocaleLong { get; init; } = null!;
  public string DefaultUsername { get; init; } = null!;
  public string DefaultEmail { get; init; } = null!;
  public string DefaultPassword { get; init; } = null!;
  public int DefaultRoleId { get; init; }
  public RoleSettings[] RoleSettings { get; init; } = null!;
}

public sealed class RoleSettings
{
  public int Id { get; init; } = 1;
  public string Name { get; init; } = null!;
  public string Description { get; init; } = null!;
}
