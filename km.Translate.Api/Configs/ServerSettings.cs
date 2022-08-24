// ReSharper disable UnusedMember.Global

namespace km.Translate.Api.Configs;

public class ServerSettings
{
  public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
  public string CorsPolicyName { get; set; } = "AllowFrontEnd";
}
