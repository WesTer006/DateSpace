using BusinessLogicLayer;
using DataAccessLayer;
using DateSpaceWebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();

builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddDataAccessDependencies(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
