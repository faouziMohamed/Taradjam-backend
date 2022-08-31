using km.Library.Utils;
using km.Translate.DataLib.Configs.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

// ReSharper disable UnusedType.Global

namespace km.Translate.DataLib.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
  public ApplicationDbContext CreateDbContext(string[] args)
  {
    bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    var configuration = Utils.GetConfig<DbConnectionSetting>(isDevelopment, "dataSettings.json");
    var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
    builder.UseSqlServer(configuration.ConnectionString,
      b =>
      {
        int maxRetries = configuration.MaxRetryAttempts;
        int retryDelay = configuration.RetryDelay;
        maxRetries = maxRetries < 0 ? 0 : maxRetries;
        retryDelay = retryDelay < 0 ? 0 : retryDelay;
        b.EnableRetryOnFailure(maxRetries, maxRetryDelay: TimeSpan.FromSeconds(retryDelay), null);
      }
    );

    return new ApplicationDbContext(builder.Options);
  }
}
