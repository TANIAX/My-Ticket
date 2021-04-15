using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.EditMyProfile
{
    public class EditMyProfileCommandValidator : AbstractValidator<EditMyProfileCommand>
    {
        public EditMyProfileCommandValidator()
        {
            RuleFor(v => v.BirthDate)
                .MustAsync(NotFuturDate).WithMessage("Birth date must be in the past");

            RuleFor(v => v.Country)
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

            RuleFor(v => v.District)
                .MaximumLength(50).WithMessage("District must not exceed 50 characters.");

            RuleFor(v => v.Locality)
                .MaximumLength(50).WithMessage("Locality must not exceed 50 characters.");

            RuleFor(v => v.Street)
                .MaximumLength(255).WithMessage("Street must not exceed 255 characters.");

            RuleFor(v => v.FirstName)
                .MinimumLength(3).WithMessage("First name must exceed 3 characters.")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters.");

            RuleFor(v => v.LastName)
                .MinimumLength(3).WithMessage("FiLastrst name must exceed 3 characters.")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.");

            RuleFor(v => v.Password)
                .MaximumLength(255).WithMessage("Password must not exceed 255 characters.");

            RuleFor(v => v.PhoneNumber)
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.");

            RuleFor(v => v.ZipCode)
                .MustAsync(IsNumber).WithMessage("Zip code must be numeric");
        }

        private async Task<bool> IsNumber(string zipCode, CancellationToken cancellationToken)
        {
            if (zipCode == null || zipCode == "")
                return true;
            return Regex.IsMatch(zipCode, @"^\d+$");
        }

        private async Task<bool> NotFuturDate(DateTime? birthDate, CancellationToken cancellationToken)
        {
            int result;
            DateTime today;

            if (birthDate == null)
                return true;

            today = DateTime.Now;
            result = DateTime.Compare(today, (DateTime)birthDate);

            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
