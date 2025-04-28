using BusinessLogicLayer;
using DataAccessLayer;
using DateSpaceWebAPI.Extensions;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

bool useLocalDb = args.Contains("UseLocalDb", StringComparer.OrdinalIgnoreCase);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();

builder.Services.AddDataAccessDependencies(builder.Configuration, useLocalDb);

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Strict,
	HttpOnly = HttpOnlyPolicy.Always,
	Secure = CookieSecurePolicy.Always
});


app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

await app.RunAsync();
