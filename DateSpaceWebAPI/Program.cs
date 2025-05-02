using DateSpaceWebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencies(builder.Configuration);

var app = builder.Build();

app.UseApplicationMiddlewares();

app.MapControllers();

await app.RunAsync();
