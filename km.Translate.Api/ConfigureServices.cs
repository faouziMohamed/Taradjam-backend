using System.Reflection;
using km.Library.Utils;
using km.Translate.Api.Configs;
using km.Translate.DataLib;
using km.Translate.DataLib.Configs.Settings;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace km.Translate.Api;

static public class ConfigureServices
{
  static public IServiceCollection AddServices(this IServiceCollection services)
  {
    // Add services to the container.
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    AddSwaggerService(services);
    AddCorsService(services);
    AddDbContextService(services);
    services.AddTransient<IUnitOfWork, UnitOfWork>();
    services.AddMediatR(typeof(MediatREntryPoint).Assembly);
    return services;
  }

  # region Services methods
  private static void AddDbContextService(IServiceCollection services)
  {
    var dbConSettings = Utils.GetConfig<DbConnectionSetting>(Utils.IsAspDevelopment());
    services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(dbConSettings.ConnectionString,
        b =>
        {
          int maxRetries = dbConSettings.MaxRetryAttempts;
          int retryDelay = dbConSettings.RetryDelay;
          maxRetries = maxRetries < 0 ? 0 : maxRetries;
          retryDelay = retryDelay < 0 ? 0 : retryDelay;
          b.EnableRetryOnFailure(maxRetries, maxRetryDelay: TimeSpan.FromSeconds(retryDelay), null);
        }
      )
    );
  }
  private static void AddCorsService(IServiceCollection services)
  {
    var serverSettings = Utils.GetConfig<ServerSettings>(Utils.IsAspDevelopment());

    services.AddCors(options =>
      {
        options.AddPolicy(
          serverSettings.CorsPolicyName,
          policy =>
          {
            policy
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins(serverSettings.AllowedOrigins);
          }
        );
      }
    );
  }

  private static void AddSwaggerService(IServiceCollection services)
  {
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    var serverDoc = Utils.GetConfig<ServerDocSettings>(Utils.IsAspDevelopment());
    services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc(
          serverDoc.Version,
          info: new OpenApiInfo
          {
            Title = serverDoc.Title,
            Version = serverDoc.Version,
            Description = serverDoc.Description,
            Contact = new OpenApiContact
            {
              Name = serverDoc.Contact.Name,
              Email = serverDoc.Contact.Email,
              Url = new Uri(serverDoc.Contact.Url)
            },
            License = new OpenApiLicense
            {
              Name = serverDoc.License.Name,
              Url = new Uri(serverDoc.License.Url)
            }
          }
        );

        var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
      }
    );
  }
  #endregion Services methods

}
