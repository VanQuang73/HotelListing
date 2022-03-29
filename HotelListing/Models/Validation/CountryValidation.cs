using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models.Validation
{
    public class CountryValidation : AbstractValidator<CountryDTO>
    {
        public CountryValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Địa chỉ không được để trống.")
                .MaximumLength(200).WithMessage("Địa chỉ không được vượt quá 200 ký tự.");
            RuleFor(x => x.ShortName).NotEmpty().WithMessage("Tên không được để trống.")
                .MaximumLength(2).WithMessage("Tên không được vượt quá 2 ký tự.");
        }
    }
}
