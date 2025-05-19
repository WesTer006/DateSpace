using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddDataAccessDependencies(this IServiceCollection services,
	    IConfiguration configuration, bool useLocal)
		{
			string connectionString;

			if (useLocal)
			{
				connectionString = configuration.GetConnectionString("SqlServerConnection");
				Console.WriteLine("Using local database connection string.");
			}
			else
			{
				connectionString = GetConnectionStringFromAzureKeyVault(configuration);
				Console.WriteLine("Using Azure Key Vault connection string.");
			}

			return services
				.AddCustomDbContext(connectionString)
                .AddScoped<IUnitOfWork, UnitOfWork>()
				.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRecommendationRepository, RecommendationRepository>();
        }
	
		private static IServiceCollection AddCustomDbContext(this IServiceCollection services, string? connectionString)
		{
			return services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));
		}

		private static string GetConnectionStringFromAzureKeyVault(IConfiguration configuration)
		{
			var secretUrl = configuration["ConnectionStrings:SecretSqlServerConnection"];
			if (string.IsNullOrEmpty(secretUrl))
				throw new InvalidOperationException("SecretSqlServerConnection URL is missing from configuration.");

			var uri = new Uri(secretUrl);
			var vaultBaseUrl = $"{uri.Scheme}://{uri.Host}";
			var segments = uri.Segments;
			if (segments.Length < 3)
				throw new FormatException("Invalid Key Vault secret URL format. Expected format: https://<vault>/secrets/<name>/<version>");

			var secretName = segments[2].TrimEnd('/');

			var client = new SecretClient(new Uri(vaultBaseUrl), new DefaultAzureCredential());
			var secret = client.GetSecret(secretName);

			return secret.Value.Value;
		}
	}
}