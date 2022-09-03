using km.Library.Utils;
using km.Translate.Api;
using km.Translate.Api.Configs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices();
var app = builder.Build();

bool isDevelopment = builder.Environment.IsDevelopment();
var serverDoc = Utils.GetConfig<ServerDocSettings>(isDevelopment);
var serverSettings = Utils.GetConfig<ServerSettings>(isDevelopment);

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
