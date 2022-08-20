using km.Translate.DataLib.Configs.Settings;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConSettings = DbConnectionSetting.GetConfig<DbConnectionSetting>("appsettings.json");
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


#region Repositories
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
