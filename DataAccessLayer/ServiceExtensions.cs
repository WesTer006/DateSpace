﻿using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DataAccessLayer
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddDataAccessDependencies(this IServiceCollection services,
	    IConfiguration configuration)
		{
			return services
				.AddCustomDbContext(configuration.GetConnectionString("SqlServerConnection"));
		}
		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, string? connectionString)
		{
			return services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connectionString));
		}
	}
}
