namespace km.Translate.DataLib.Configs.Settings;

public sealed class DbConnectionSetting
{
  public int MaxRetryAttempts { get; set; }
  public int RetryDelay { get; set; }
  public string ConnectionString { get; set; } = string.Empty;
}
