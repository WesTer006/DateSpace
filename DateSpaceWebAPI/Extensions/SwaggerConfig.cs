using System.Reflection;
using Microsoft.OpenApi.Models;

namespace DateSpaceWebAPI.Extensions
{
    /// <summary>
    /// Клас для налаштування Swagger в додатку.
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Додає налаштування Swagger до контейнера служб.
        /// </summary>
        /// <param name="services">Інстанс контейнера служб</param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DateSpace WebAPI",
                    Version = "v1",
                    Description = "API для дейтінг-додатка"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введіть токен в форматі: Bearer {твій_токен}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        /// <summary>
        /// Налаштовує Swagger для додатку.
        /// </summary>
        /// <param name="app">Інстанс веб-додатку</param>
        public static void UseSwaggerConfiguration(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DateSpace API v1");
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger");
                    return;
                }
                await next();
            });
        }
    }
}
