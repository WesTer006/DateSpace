using Shared.DTOs;
using FluentValidation;

namespace Shared.Validators
{
	public class UserProfileDtoValidator : AbstractValidator<UserProfileDto>
	{
		public UserProfileDtoValidator()
		{
			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("Username is required.")
				.MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

			RuleFor(x => x.Age)
				.GreaterThan(0).WithMessage("Age must be a positive number.")
				.LessThanOrEqualTo(120).WithMessage("Age must be realistic.");

			RuleFor(x => x.Gender)
				.NotEmpty().WithMessage("Gender is required.")
				.Must(g => g == "Male" || g == "Female" || g == "Other")
				.WithMessage("Gender must be 'Male', 'Female' or 'Other'.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(x => x.Bio)
				.MaximumLength(500).WithMessage("Bio can't be longer than 500 characters.");

		}
	}
}
