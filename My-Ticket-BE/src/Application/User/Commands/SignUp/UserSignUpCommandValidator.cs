using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.User.Commands.SignUp
{
    public class UserSignUpCommandValidator : AbstractValidator<UserSignUpCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmail _email;

        public UserSignUpCommandValidator(IApplicationDbContext context, IEmail email)
        {
            _context = context;
            _email = email;

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .MinimumLength(5).WithMessage("Email must exceed 5 characters.")
                .Must(BeUniqueEmail).WithMessage("Email already taken.")
                .Must(ValidMail).WithMessage("Email is not valid.");

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters.")
                .MinimumLength(3).WithMessage("First name must exceed 3 characters.");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.")
                .MinimumLength(3).WithMessage("Last name must exceed 3 characters.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
                .MinimumLength(6).WithMessage("Password must exceed 6 characters.");
        }

        private bool BeUniqueEmail(string email)
        {
            if (_context.User.FirstOrDefault(x => x.UserName == email) == null)
                return true;
            else
                return false;
        }

        private bool ValidMail(string email)
        {
            return _email.MailIsValid(email);
        }
    }
}
