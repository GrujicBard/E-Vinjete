using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using VignetteAuth.Controllers;
using VignetteAuth.DataAccess;
using VignetteAuth.Settings;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddSingleton<IMongoDbSettings>(provider =>
     provider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddControllers();

builder.Services.AddSingleton<UserDataAccess>();

builder.Services.AddGrpc();

builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserGrpcController>();
});

app.Run();

public partial class Program { }