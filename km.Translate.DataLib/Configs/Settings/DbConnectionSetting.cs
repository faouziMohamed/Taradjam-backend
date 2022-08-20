using Microsoft.Extensions.Configuration;

namespace km.Translate.DataLib.Configs.Settings;

public sealed class DbConnectionSetting
{
  public int MaxRetryAttempts { get; set; }
  public int RetryDelay { get; set; }
  public string ConnectionString { get; set; } = string.Empty;
  static public TSettings GetConfig<TSettings>(string configFile)
  {
    return new ConfigurationBuilder()
      .AddJsonFile(configFile)
      .Build()
      .GetRequiredSection(typeof(TSettings).Name)
      .Get<TSettings>();
  }
}
