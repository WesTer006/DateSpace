using FluentValidation;
using Shared.Validators;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;

namespace Shared
{
	public static class ValidatorExtensions
	{
		public static IServiceCollection AddFluentValidation(this IServiceCollection services)
		{

			return services.AddFluentValidationAutoValidation()
				.AddFluentValidationClientsideAdapters()
				.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
		}
		
	}
}
