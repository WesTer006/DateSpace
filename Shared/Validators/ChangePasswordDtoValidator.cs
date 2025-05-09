using FluentValidation;
using Shared.DTOs;


namespace Shared.Validators
{
	public class ChangePasswordDtoValidator:AbstractValidator<ChangePasswordDto>
	{
		public ChangePasswordDtoValidator()
		{
             RuleFor(x => x.NewPassword)
			    .NotEmpty()
			    .MinimumLength(6)
			    .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
			    .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
			    .Matches("[0-9]").WithMessage("Password must contain a digit");
		}
		
	}
}
