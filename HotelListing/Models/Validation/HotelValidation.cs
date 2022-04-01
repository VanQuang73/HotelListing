using FluentValidation;
using HotelListing.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models.Validation
{
    public class CreateHotelValidation : AbstractValidator<CreateHotelDTO>
    {
        public CreateHotelValidation()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage(string.Format(Resource.VALIDATION_NOT_EMPTY, "Địa chỉ"))
                .MinimumLength(2).WithMessage(string.Format(Resource.VALIDATION_MIN_LENGTH, "Địa chỉ", "2"))
                .MaximumLength(200).WithMessage(string.Format(Resource.VALIDATION_MAX_LENGTH, "Địa chỉ", "200"));

            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Resource.VALIDATION_NOT_EMPTY, "Tên"))
                .MinimumLength(2).WithMessage(string.Format(Resource.VALIDATION_MIN_LENGTH, "Tên", "2"))
                .MaximumLength(200).WithMessage(string.Format(Resource.VALIDATION_MAX_LENGTH, "Tên", "200"));
        }
    }
}