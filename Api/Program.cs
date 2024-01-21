using Api.Models;
using Api.Store;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var edmModelBuilder = new ODataConventionModelBuilder();
edmModelBuilder.EnableLowerCamelCase();
edmModelBuilder.Namespace = "ODataSample";
edmModelBuilder.ContainerName = "ODataSample";
edmModelBuilder.EntitySet<Product>("Product");
edmModelBuilder.EntitySet<Customer>("Customer");
edmModelBuilder.EntitySet<Order>("Order");

services.AddControllers().AddOData(options =>
{
  options
    .Select().Filter().OrderBy()
    .Expand().Count().SetMaxTop(null)
    .AddRouteComponents("odata", edmModelBuilder.GetEdmModel());
});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpClient();
services.AddSerilog();
services.AddScoped<IDataStore, DataStore>();

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