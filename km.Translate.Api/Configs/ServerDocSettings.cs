using System.ComponentModel.DataAnnotations;

// ReSharper disable ClassNeverInstantiated.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace km.Translate.Api.Configs;

public class ServerDocSettings
{
  public string Version { get; set; }

  public string Title { get; set; }

  public string Description { get; set; }

  public ContactSettings Contact { get; set; }

  public LicenseSettings License { get; set; }
}

public class LicenseSettings
{
  public string Name { get; set; }

  [Url]
  public string Url { get; set; }
}

public class ContactSettings
{
  public string Name { get; set; }

  [Url]
  public string Url { get; set; }

  [EmailAddress]
  public string Email { get; set; }
}
