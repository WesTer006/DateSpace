using BusinessLogicLayer;
using DataAccessLayer;
using DateSpaceWebAPI.Extensions;
using Microsoft.AspNetCore.CookiePolicy;
using Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDataAccessDependencies(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddFluentValidation();
builder.Services.AddAutoMapperConfiguration();

builder.Services.AddSwaggerConfiguration();


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
