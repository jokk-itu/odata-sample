using Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.ConfigureAll<HttpClientFactoryOptions>(options =>
{
  options.HttpMessageHandlerBuilderActions.Add(builder =>
  {
    builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<PerformanceRequestHandler>());
  });
});
services.AddSwaggerGen();
services.AddHttpClient();
services.AddSerilog();

var serilogConfiguration = configuration.GetSection("Serilog");
Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .Enrich.WithProcessName()
  .Enrich.WithProcessId()
  .Enrich.WithThreadName()
  .Enrich.WithThreadId()
  .Enrich.WithMemoryUsage()
  .Enrich.WithProperty("ContainerId", Environment.GetEnvironmentVariable("HOSTNAME"))
  .WriteTo.Console()
  .WriteTo.Seq(serilogConfiguration.GetSection("Seq").GetValue<string>("Url"))
  .CreateBootstrapLogger();

try
{
  var app = builder.Build();
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseSerilogRequestLogging();
  app.UseAuthorization();
  app.MapControllers();
  app.Run();
}
catch(Exception e)
{
  Log.Error(e, "Unexpected error occurred");
}
finally
{
  await Log.CloseAndFlushAsync();
}