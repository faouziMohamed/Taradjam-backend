using km.Translate.Data.Data;
using km.Translate.Data.Repositories;
using km.Translate.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(connectionString: builder.Configuration.GetValue<string>("Database:ConnectionString"),
    b =>
    {
      var maxRetries = builder.Configuration.GetValue<int>("Database:MaxRetryAttempts");
      var retryDelay = builder.Configuration.GetValue<double>("Database:RetryDelay");
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
