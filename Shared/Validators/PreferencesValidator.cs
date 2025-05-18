using FluentValidation;
using Shared.DTOs;

namespace Shared.Validators
{
	public class PreferencesValidator : AbstractValidator<PreferenceDto>
	{
		public PreferencesValidator()
		{
			var allowedGenders = new[] { "Male", "Female", "Other" };

			RuleFor(x => x.PreferredGender)
				.NotEmpty().WithMessage("PreferredGender is required.")
				.Must(gender => allowedGenders.Contains(gender))
				.WithMessage("PreferredGender must be one of: Male, Female, Other.");

			RuleFor(x => x.MinAge)
				.InclusiveBetween(18, 149)
				.WithMessage("MinAge must be between 18 and 149.");

			RuleFor(x => x.MaxAge)
				.InclusiveBetween(19, 150)
				.WithMessage("MaxAge must be between 19 and 150.");

			RuleFor(x => x)
				.Must(x => x.MinAge <= x.MaxAge)
				.WithMessage("MinAge cannot be greater than MaxAge.");

			RuleFor(x => x.MaxDistance)
				.InclusiveBetween(1, 1000)
				.WithMessage("MaxDistance must be between 1 and 1000 kilometers.");
		}
	}
}
