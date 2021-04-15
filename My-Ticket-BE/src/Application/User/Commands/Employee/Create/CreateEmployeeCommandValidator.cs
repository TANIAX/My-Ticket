using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Application.User.Commands.Employee.Create
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmail _email;
        public CreateCustomerCommandValidator(IApplicationDbContext context, IEmail email)
        {
            _context = context;
            _email = email;

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MinimumLength(5).WithMessage("Email must exceed 5 characters.")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters")
                .Must(BeUniqueEmail).WithMessage("Email already taken.")
                .Must(ValidMail).WithMessage("Email is not valid.");

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(3).WithMessage("First name must exceed 3 characters")
                .MaximumLength(20).WithMessage("First name must not exceed 20 characters");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(3).WithMessage("Last name must exceed 3 characters")
                .MaximumLength(20).WithMessage("Last name must not exceed 20 characters");

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
