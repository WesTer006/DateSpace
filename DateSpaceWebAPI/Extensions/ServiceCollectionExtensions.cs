using BusinessLogicLayer;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer;
using Microsoft.AspNetCore.CookiePolicy;
using Shared;

namespace DateSpaceWebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            bool useLocalDb = configuration.GetValue<bool>("UseLocalDb");

            services.AddSwaggerConfiguration();
            services.AddControllers();
			services.AddFluentValidation();
			services.AddDataAccessDependencies(configuration, useLocalDb);
            services.AddIdentityServices(configuration);
            services.AddAutoMapperConfiguration();
            services.AddJwtAuthentication(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            services.AddScoped<IPreferenceService, PreferenceService>();
            services.AddScoped<IRecommendationService, RecommendationService>();

            return services;
        }

        public static WebApplication UseApplicationMiddlewares(this WebApplication app)
        {
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

            return app;
        }
    }
}
