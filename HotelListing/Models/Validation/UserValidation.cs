using FluentValidation;
using HotelListing.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models.Validation
{
    public class UserValidation : AbstractValidator<UserDTO>
    {
        public UserValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(string.Format(Resource.VALIDATION_NOT_EMPTY, "Email"))
                .EmailAddress().WithMessage(string.Format(Resource.VALIDATION_DISPLAY, "Email"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(string.Format(Resource.VALIDATION_NOT_EMPTY, "Password"));
        }
    }

    public class ResetPasswordValidation : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(string.Format(Resource.VALIDATION_NOT_EMPTY, "Email"))
                .EmailAddress().WithMessage(string.Format(Resource.VALIDATION_DISPLAY, "Email"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(string.Format(Resource.VALIDATION_NOT_EMPTY, "Password"))
                .MaximumLength(200).WithMessage(string.Format(Resource.VALIDATION_MAX_LENGTH, "Password", "200"));
            RuleFor(x => x).Custom((request, context) =>
            {
                if(request.Password != request.ConfirmPassword)
                {
                    context.AddFailure(string.Format(Resource.VALIDATION_COMPARE, "Password"));
                }
            });
        }
    }
}
