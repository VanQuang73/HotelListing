using FluentValidation;
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
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email sai định dạng.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được để trống.");
        }
    }

    public class ResetPasswordValidation : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email sai định dạng.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .Length<ResetPassword>(6, 200).WithMessage("Mật khẩu lớn hơn 6 và nhỏ hơn 200 ký tự.");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Mật khẩu chưa khớp.");
                }
            });
        }
    }
}