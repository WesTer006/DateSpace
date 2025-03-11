using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
