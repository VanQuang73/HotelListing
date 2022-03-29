using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models.Validation
{
    public class HotelValidation : AbstractValidator<HotelDTO>
    {
        public HotelValidation()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được để trống.")
                .MaximumLength(200).WithMessage("Địa chỉ không được vượt quá 200 ký tự.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(200).WithMessage("Tên không được vượt quá 200 ký tự.");
        }
    }
}
