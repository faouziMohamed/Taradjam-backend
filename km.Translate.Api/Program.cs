using System.Reflection;
using km.Library.Utils;
using km.Translate.Api.Configs;
using km.Translate.DataLib.Configs.Settings;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
bool isDevelopment = builder.Environment.IsDevelopment();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var serverDoc = Utils.GetConfig<ServerDocSettings>(isDevelopment);
builder.Services.AddSwaggerGen(options =>
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

var serverSettings = Utils.GetConfig<ServerSettings>(isDevelopment);
builder.Services.AddCors(options =>
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

var dbConSettings = Utils.GetConfig<DbConnectionSetting>(isDevelopment);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
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

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(
  options =>
  {
    options.DocumentTitle = serverDoc.Title;
    options.SwaggerEndpoint(url: $"/swagger/{serverDoc.Version}/swagger.json", serverDoc.Title);
    options.RoutePrefix = string.Empty;
  }
);

app.UseCors(serverSettings.CorsPolicyName);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
